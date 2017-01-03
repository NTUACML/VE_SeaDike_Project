#include "../include/Module2_Internal.h"


Module2_Internal::Module2_Internal():Var(NULL)
{
}


Module2_Internal::~Module2_Internal()
{
}

void Module2_Internal::SetVar(Module2_Var * _Var)
{
	Var = _Var;
}

bool Module2_Internal::GeoPreCal()
{
	Var->Ref_x = 0.0;
	Var->Ref_y = 0.0;
	//Block Preprocess
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->BlockData[i].Cal_Area();
		Var->BlockData[i].Cal_WeightC();
		Var->BlockData[i].Cal_MinMax();
		Var->Ref_x += Var->BlockData[i].WeightC.x;
		Var->Ref_y += Var->BlockData[i].WeightC.y;


	}
	Var->Ref_x /= double(Var->BlockData.size());
	Var->Ref_y /= double(Var->BlockData.size());

	//Get Max and Min level of All Block
	Var->Max_level = Var->Ref_y;
	Var->Min_level = Var->Ref_y;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Var->Min_level) Var->Min_level = Var->BlockData[i].MinLevel;
		if (Var->BlockData[i].MaxLevel >= Var->Max_level) Var->Max_level = Var->BlockData[i].MaxLevel;
	}

	////Get EL Level Up block ID and Arm Y
	//for (size_t i = 0; i < Var->LevelSection.size() - 1; i++)
	//{
	//	for (size_t j = 0; j < Var->BlockData.size(); j++)
	//	{
	//		if (Var->BlockData[j].WeightC.y >= Var->LevelSection[i].Level &&
	//			Var->BlockData[j].WeightC.y < Var->LevelSection[i + 1].Level)
	//		{
	//			Var->LevelSection[i].BlockId.push_back(j);
	//		}
	//	}
	//}

	//Base Block Length
	double BaseMin_x, BaseMax_x, Min_weight_y;
	BaseMin_x = Var->Ref_x;
	BaseMax_x = Var->Ref_x;
	Min_weight_y = Var->Ref_y;

	//- Find Min Weight Y
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].WeightC.y <= Min_weight_y) Min_weight_y = Var->BlockData[i].WeightC.y;
	}

	//- Find Node max_x and min_x lower than Weight Y
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Min_weight_y) {
			for (size_t j = 0; j < Var->BlockData[i].Node.size(); j++) {
				if (Var->BlockData[i].Node[j].x <= BaseMin_x) BaseMin_x = Var->BlockData[i].Node[j].x;
				if (Var->BlockData[i].Node[j].x >= BaseMax_x) BaseMax_x = Var->BlockData[i].Node[j].x;
			}
		}
	}

	Var->B = std::abs(BaseMax_x - BaseMin_x);

	Var->Err_Msg += "計算幾何前處理完畢! \r\n";
	return true;
}

bool Module2_Internal::WeightCal()
{
	double eps = 1e-3;
	// Find Ref x with different direction
	double Ref_x = Var->Ref_x;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Ref_x = Var->BlockData[i].MinX;
	}

	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->BlockData[i].SelfWeight = Var->BlockData[i].Area * Var->BlockData[i].Density;
		Var->BlockData[i].X = std::abs(Var->BlockData[i].WeightC.x);
		Var->BlockData[i].Mw = Var->BlockData[i].SelfWeight * Var->BlockData[i].X;
	}
	//- Sum Total Weight and Moment
	Var->W = 0.0;
	Var->Mw = 0.0;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->W += Var->BlockData[i].SelfWeight;
		Var->Mw += Var->BlockData[i].Mw;
	}

	Var->Err_Msg += "塊體自重力計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::EarthQuakeForceCal()
{
	double eps = 1e-3;
	// Find Ref x with different direction
	double Ref_x = Var->Ref_x;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Ref_x = Var->BlockData[i].MinX;
	}
	

	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].Density == 2.3 || Var->BlockData[i].Density == 1.27)
		{
			Var->BlockData[i].Density = 2.3;
		}
		else if (Var->BlockData[i].Density == 1.8 || Var->BlockData[i].Density == 1.0) {
			Var->BlockData[i].Density = 1.8;
		}
		//-此處的Selfweight為地震力Fe Mw為地震的傾倒彎矩
		Var->BlockData[i].SelfWeight = Var->BlockData[i].Area * Var->BlockData[i].Density * Var->K;
		Var->BlockData[i].X = std::abs(Var->BlockData[i].WeightC.y);
		Var->BlockData[i].Mw = Var->BlockData[i].SelfWeight * Var->BlockData[i].X;
	}
	//- Sum Total Weight and Moment
	Var->W = 0.0;
	Var->Mw = 0.0;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->Fe += Var->BlockData[i].SelfWeight;
		Var->Me += Var->BlockData[i].Mw;
	}

	Var->Err_Msg += "塊體地震力計算處理完畢! \r\n";
	return true;
}