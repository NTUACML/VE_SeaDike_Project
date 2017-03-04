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

	//Get EL Level Up block ID and Arm Y

	double maximum_X = 0.0;
	std::vector<size_t> tmp_blockid;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{

		for (size_t j = 0; j < Var->BlockData.size(); j++)
		{
			if (Var->BlockData[j].CalMoment == true && 
				Var->BlockData[j].MinLevel >= Var->LevelSection[i].Level &&
				Var->BlockData[j].MaxX >= maximum_X)
			{
				maximum_X = Var->BlockData[j].MaxX;
			}
		}

		for (size_t j = 0; j < Var->BlockData.size(); j++)
		{
			if (Var->BlockData[j].MinLevel >= Var->LevelSection[i].Level &&
				Var->BlockData[j].MaxX <= maximum_X &&
				std::find(tmp_blockid.begin(), tmp_blockid.end(), j) == tmp_blockid.end())
			{
				Var->LevelSection[i].BlockId.push_back(j);
				tmp_blockid.push_back(j);
			}
		}
	}

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
	size_t ID;
	double temp_sum_Mx = 0;
	double temp_sum_W = 0;

	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		ID = Var->LevelSection[i].BlockId[0];
		double Ref_x = Var->BlockData[ID].MinX;

		for (size_t j = 1; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			if (Var->BlockData[ID].MinX<Ref_x) Ref_x = Var->BlockData[ID].MinX;
		}

		//- 更新下一個elevation的力臂與抵抗彎矩(但仍需考慮新設一變數已儲存兩個不同elevation之總和)
		if (i >= 1) {
			Var->LevelSection[i].pre_sum_W = Var->LevelSection[i - 1].Level_sum_W;
			Var->LevelSection[i].pre_total_arm = Var->LevelSection[i - 1].Level_total_arm-Ref_x;
			Var->LevelSection[i].pre_sum_Mx = Var->LevelSection[i].pre_sum_W*Var->LevelSection[i].pre_total_arm;
			temp_sum_Mx = Var->LevelSection[i].pre_sum_Mx;
			temp_sum_W = Var->LevelSection[i].pre_sum_W;
		}


		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			Var->BlockData[ID].SelfWeight = Var->BlockData[ID].Area * Var->BlockData[ID].Density;
			Var->BlockData[ID].X = std::abs(Var->BlockData[ID].WeightC.x - Ref_x);
			Var->BlockData[ID].Mw = Var->BlockData[ID].SelfWeight * Var->BlockData[ID].X;

			//- Temperary summation ofevery level
			temp_sum_W += Var->BlockData[ID].SelfWeight;
			temp_sum_Mx += Var->BlockData[ID].Mw;
		}

		Var->LevelSection[i].Level_sum_W = temp_sum_W;
		Var->LevelSection[i].Level_sum_Mx = temp_sum_Mx;

		Var->LevelSection[i].Level_total_arm = Var->LevelSection[i].Level_sum_Mx / Var->LevelSection[i].Level_sum_W;

	}

	Var->W = temp_sum_W;
	Var->Mw = temp_sum_Mx;

	Var->Err_Msg += "塊體自重力計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::EarthQuakeForceCal()
{
	size_t ID;
	double temp_sum_Me = 0;
	double temp_sum_Fe = 0;

	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		ID = Var->LevelSection[i].BlockId[0];
		double old_Ref_y;
		double Ref_y = Var->BlockData[ID].MinLevel;

		for (size_t j = 1; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];
			if (Var->BlockData[ID].MinLevel<Ref_y) Ref_y = Var->BlockData[ID].MinLevel;
		}

		//- 更新下一個elevation的力臂與抵抗彎矩(但仍需考慮新設一變數已儲存兩個不同elevation之總和)
		if (i >= 1) {
			Var->LevelSection[i].pre_sum_WE = Var->LevelSection[i - 1].Level_sum_WE;
			double Arm_len = abs(old_Ref_y - Ref_y);
			Var->LevelSection[i].pre_total_armE = Var->LevelSection[i - 1].Level_total_armE + Arm_len;
			Var->LevelSection[i].pre_sum_MxE = Var->LevelSection[i].pre_sum_WE*Var->LevelSection[i].pre_total_armE;
			temp_sum_Me = Var->LevelSection[i].pre_sum_MxE;
			temp_sum_Fe = Var->LevelSection[i].pre_sum_WE;
		}

		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			ID = Var->LevelSection[i].BlockId[j];

			//-暫時將所有單位體重都換成未浸水
			/*if (Var->BlockData[ID].Density == 2.3 || Var->BlockData[ID].Density == 1.27)
			{
				Var->BlockData[ID].Density = 2.3;
			}
			else if (Var->BlockData[ID].Density == 1.8 || Var->BlockData[ID].Density == 1.0) {
				Var->BlockData[ID].Density = 1.8;
			}*/
			Var->BlockData[ID].SelfWeight = Var->BlockData[ID].Area * Var->BlockData[ID].EQ_Density* Var->K;
			Var->BlockData[ID].X = std::abs(Var->BlockData[ID].WeightC.y - Ref_y);
			Var->BlockData[ID].Mw = Var->BlockData[ID].SelfWeight * Var->BlockData[ID].X;

			//- Temperary summation ofevery level
			temp_sum_Fe += Var->BlockData[ID].SelfWeight;
			temp_sum_Me += Var->BlockData[ID].Mw;
		}

		Var->LevelSection[i].Level_sum_WE = temp_sum_Fe;
		Var->LevelSection[i].Level_sum_MxE = temp_sum_Me;

		Var->LevelSection[i].Level_total_armE = Var->LevelSection[i].Level_sum_MxE / Var->LevelSection[i].Level_sum_WE;

		old_Ref_y = Ref_y;
	}
	
	//- Sum Total Weight and Moment
	Var->Fe = temp_sum_Me;
	Var->Me = temp_sum_Fe;

	Var->Err_Msg += "塊體地震力計算處理完畢! \r\n";
	return true;
}

bool Module2_Internal::HorizontalSoilForceCal() {

	double pi_up, pi_down;
	double rh = 0;
	pi_down = (Var->Q + rh)*Var->ka*cos(Var->WallPhi*M_PI / 180);


	Var->Err_Msg += "土壓水平力及傾倒彎矩計算處理完畢! \r\n";
	return true;
}
//double F_Sum = 0.0;
//for (size_t i = 1; i < Var->LevelSection.size(); i++)
//{
//	Dis_Face = Var->LevelSection[i].Level - Var->LevelSection[i - 1].Level;
//	Var->LevelSection[i].FP = (Var->LevelSection[i].P + Var->LevelSection[i - 1].P)  * Dis_Face / 2.0;
//
//	// Tragngle Used
//	F_Sum = Var->LevelSection[i - 1].P * Dis_Face * (Dis_Face / 2.0) + (Var->LevelSection[i].P - Var->LevelSection[i - 1].P) * (Dis_Face / 2.0) * (2.0 * Dis_Face / 3.0);
//
//	Var->LevelSection[i].L_Y = (Var->LevelSection[i - 1].Level - Var->LevelSection.begin()->Level) + F_Sum / Var->LevelSection[i].FP;
//
//	Var->LevelSection[i].Mp = Var->LevelSection[i].FP * Var->LevelSection[i].L_Y;
//}
//Var->LevelSection.end()->FP = 0.0;
//Var->LevelSection.end()->Mp = 0.0;