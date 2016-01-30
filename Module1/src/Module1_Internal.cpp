#include "../include/Module1_Internal.h"

Module1_Internal::Module1_Internal():Var(NULL)
{
}

Module1_Internal::~Module1_Internal()
{
}

void Module1_Internal::SetVar(Module1_Var * _Var)
{
	Var = _Var;
}

bool Module1_Internal::GeoPreCal()
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

	//Get Shift Ref point to Min (Max) Coord. and Get Sea Side
	double SeaSide_Cut_X;
	SeaSide_Cut_X = Var->Ref_x;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->Direction == 1) { //Wave from RHS
			if (Var->BlockData[i].MinX <= Var->Ref_x) Var->Ref_x = Var->BlockData[i].MinX;
			if (Var->BlockData[i].MinLevel <= Var->Ref_y) Var->Ref_y = Var->BlockData[i].MinLevel;

			if (Var->BlockData[i].WeightC.x >= SeaSide_Cut_X) SeaSide_Cut_X = Var->BlockData[i].WeightC.x;
		}
		else if (Var->Direction == 0) { //Wave from LHS
			if (Var->BlockData[i].MaxX >= Var->Ref_x) Var->Ref_x = Var->BlockData[i].MaxX;
			if (Var->BlockData[i].MinLevel <= Var->Ref_y) Var->Ref_y = Var->BlockData[i].MinLevel;

			if (Var->BlockData[i].WeightC.x <= SeaSide_Cut_X) SeaSide_Cut_X = Var->BlockData[i].WeightC.x;
		}
	}

	//Select sea side Block
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->Direction == 1) { //Wave from RHS
			if (Var->BlockData[i].MaxX >= SeaSide_Cut_X) Var->BlockData[i].OnSeaSide = true;
			else Var->BlockData[i].OnSeaSide = false;
		}
		else if (Var->Direction == 0) { //Wave from LHS
			if (Var->BlockData[i].MinX <= SeaSide_Cut_X) Var->BlockData[i].OnSeaSide = true;
			else Var->BlockData[i].OnSeaSide = false;
		}
	}

	//Get Max and Min level of block
	Var->Max_level = Var->Ref_y;
	Var->Min_level = Var->Ref_y;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Var->Min_level) Var->Min_level = Var->BlockData[i].MinLevel;
		if (Var->BlockData[i].MaxLevel >= Var->Max_level) Var->Max_level = Var->BlockData[i].MaxLevel;
	}

	//------- Need to Revise ------------
	double Max_SeaSide_coord_MaxY;
	Max_SeaSide_coord_MaxY = Var->Ref_y;
	//Get EL Level
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].OnSeaSide) {
			Var->LevelSection.emplace_back(Var->BlockData[i].MinLevel);
			if (Var->BlockData[i].MaxLevel >= Max_SeaSide_coord_MaxY) Max_SeaSide_coord_MaxY = Var->BlockData[i].MaxLevel;
		}
	}
	Var->LevelSection.emplace_back(Max_SeaSide_coord_MaxY);

	//Sort EL
	std::sort(Var->LevelSection.begin(), Var->LevelSection.end(), [](const EL & a, const EL & b) {
		return a.Level < b.Level;
	});

	//Get EL Level Up block ID and Arm Y
	for (size_t i = 0; i < Var->LevelSection.size() - 1; i++)
	{
		for (size_t j = 0; j < Var->BlockData.size(); j++)
		{
			if (Var->BlockData[j].WeightC.y >= Var->LevelSection[i].Level &&
				Var->BlockData[j].WeightC.y < Var->LevelSection[i + 1].Level)
			{
				Var->LevelSection[i].BlockId.push_back(j);
			}
		}
		Var->LevelSection[i].L_Y = ((Var->LevelSection[i + 1].Level + Var->LevelSection[i].Level) / 2.0) - Var->Ref_y;
	}

	//Base Block Length
	double BaseMin_x, BaseMax_x;
	BaseMin_x = Var->Ref_x;
	BaseMax_x = Var->Ref_x;

	for (auto & Id : Var->LevelSection.begin()->BlockId) {
		if (Var->BlockData[Id].MinX <= BaseMin_x) {
			BaseMin_x = Var->BlockData[Id].MinX;
		}
		if (Var->BlockData[Id].MaxX >= BaseMax_x) {
			BaseMax_x = Var->BlockData[Id].MaxX;
		}
	}
	Var->B = std::abs(BaseMax_x - BaseMin_x);

	//------- Need to Revise ------------



	Var->Err_Msg += "計算幾何前處理完畢! \r\n";
	return true;
}

bool Module1_Internal::WaterLevelCal()
{
	// Background water level conditions
	Var->h = Var->HWL - Var->Base_Level;

	Var->h_plun = Var->HWL - Var->Min_level;

	Var->hc = Var->Max_level - Var->HWL;

	Var->d = Var->HWL - Var->Breaker_Level;

	if (Var->d <= 0.0) Var->d = 0.0;

	Var->Err_Msg += "- 水深條件計算成功! \r\n";

	// Cal definetion of water level

	Var->L0 = 1.56 * Var->T0 * Var->T0;

	Var->H0_plun = Var->H0 * Var->Kr * Var->Kd;

	Var->h_D_L0 = Var->h / Var->L0;

	// Dispersion Relation Start
	Var->Err_Msg += "- 延散關係式計算開始! \r\n";

	double w2 = std::pow(2.0 * M_PI / Var->T0, 2.0), 
		   kh, kh_1 = (2.0 * M_PI / Var->L0) * Var->h,
		   eps = 1e-6, err, F, F_plun;
	int count = 0, Max_Itr = 10000;

	while (count <= Max_Itr)
	{
		F = (w2 * Var->h) / (M_G * kh_1) - std::tanh(kh_1);
		F_plun = -(w2 * Var->h) / (M_G * std::pow(kh_1, 2.0)) - std::pow(1.0 / std::cosh(kh_1), 2.0);
		kh = kh_1 - (F / F_plun);

		err = std::abs(kh - kh_1);
		if(err <= eps){ 
			Var->Err_Msg += "- 延散關係式計算成功! \r\n";
			Var->L = 2.0 * M_PI * Var->h / kh;
			break;
		}
		
		kh_1 = kh;	
		++count;	
	}

	if (count >= Max_Itr) {
		Var->L = 2.0 * M_PI * Var->h / kh;
		Var->Err_Msg += "- 延散關係式計算失敗! \r\n";
		Var->Err_Msg += "- 確保數值精確!程式停止計算! \r\n";
		return false;
	}
	
	
	Var->beta0 = 0.028 *std::pow(Var->H0_plun / Var->L0, -0.38) * std::exp(20.0 * std::pow(Var->S, 1.5));

	Var->beta1 = 0.52 * std::exp(4.2 * Var->S);

	Var->betaMax = std::max(0.92,
					0.32 *std::pow(Var->H0_plun / Var->L0, -0.29) * std::exp(2.4 * Var->S));

	Var->beta0_Star = 0.052 *std::pow(Var->H0_plun / Var->L0, -0.38) * std::exp(20.0 * std::pow(Var->S, 1.5));

	Var->beta1_Star = 0.63 * std::exp(4.2 * Var->S);

	Var->betaMax_Star = std::max(1.65,
		0.53 *std::pow(Var->H0_plun / Var->L0, -0.29) * std::exp(2.4 * Var->S));

	double KsHo_p;

	KsHo_p = Var->Ks * Var->H0_plun;

	if (Var->h_D_L0 >= 0.2) {
		Var->Hs = KsHo_p;
		Var->Hmax = 1.8 * KsHo_p;
	}
	else {
		Var->Hs = std::min(std::min(Var->beta0 * Var->H0_plun + Var->beta1 * Var->h, Var->betaMax * Var->H0_plun), KsHo_p);
		Var->Hmax = std::min(std::min(Var->beta0_Star * Var->H0_plun + Var->beta1_Star * Var->h, Var->betaMax_Star * Var->H0_plun), 1.8*KsHo_p);
	}

	Var->hb = Var->h + 5.0 * Var->Hs * Var->S;

	Var->Err_Msg += "背景水理資料處理完畢! \r\n";
	return true;
}

bool Module1_Internal::WavePressureCal()
{
	double tmp;

	tmp = 4.0 * M_PI * Var->h / Var->L;
	Var->alpha1 = 0.6 + 0.5 * std::pow(tmp / std::sinh(tmp), 2.0);

	tmp = ((Var->hb - Var->d) / (3.0 * Var->hb)) * std::pow(Var->Hmax / Var->d, 2.0);
	Var->alpha2 = std::min(tmp, 2.0 * Var->d / Var->Hmax);

	tmp = 1.0 / std::cosh(2.0 * M_PI * Var->h / Var->L);
	Var->alpha3 = 1.0 - (Var->h_plun / Var->h) * (1.0 - tmp);

	Var->eta_Star = 0.75 * (1.0 + std::cos(Var->beta)) * Var->lamda * Var->Hmax;

	Var->hc_Star = std::min(Var->eta_Star, Var->hc);

	Var->alpha4 = 1.0 - (Var->hc_Star / Var->eta_Star);

	Var->P1 = 0.5 * (1.0 + std::cos(Var->beta)) * (Var->alpha1 + Var->alpha2 * std::pow(std::cos(Var->beta), 2.0)) * Var->DensitySea * Var->Hmax * Var->lamda;

	Var->P2 = Var->P1 / (std::cosh(2.0 * M_PI * Var->h) / Var->L);

	Var->P3 = Var->alpha3 * Var->P1;

	Var->P4 = Var->P1 * Var->alpha4;
	// Pressure Moment.
	//- Find HWL 
	size_t HWL_ID = 0;
	double eps = 1e-3, Dis_Face, M;
	for (size_t i = 1; i < Var->LevelSection.size(); i++)
	{
		if ((Var->LevelSection[i - 1].Level - eps) < Var->HWL
			&&
			Var->HWL <= (Var->LevelSection[i].Level + eps)) {
			HWL_ID = i;
		}
	}
	//- Put Wave P magnitude
	//-- Put P3
	Var->LevelSection.begin()->P = Var->P3;
	//-- Put P4
	Var->LevelSection.end()->P = Var->P4;
	//-- Put HWL
	Var->LevelSection[HWL_ID].P = Var->P1;
	//-- Interpolation
	M = (Var->LevelSection[HWL_ID].P - Var->LevelSection.begin()->P) / (Var->LevelSection[HWL_ID].Level - Var->LevelSection.begin()->Level);
	for (size_t i = 1; i < HWL_ID; i++)
	{
		Var->LevelSection[i].P = Var->LevelSection.begin()->P + M * (Var->LevelSection[i].Level - Var->LevelSection[i - 1].Level);
	}
	M = (Var->LevelSection.end()->P - Var->LevelSection[HWL_ID].P) / (Var->LevelSection.end()->Level - Var->LevelSection[HWL_ID].Level);
	for (size_t i = HWL_ID + 1; i < Var->LevelSection.size() - 1; i++)
	{
		Var->LevelSection[i].P = Var->LevelSection[HWL_ID].P + M * (Var->LevelSection[i].Level - Var->LevelSection[i - 1].Level);
	}
	//- Wave Pressure & Moment
	for (size_t i = 0; i < Var->LevelSection.size() - 1; i++)
	{
		Dis_Face = Var->LevelSection[i + 1].Level - Var->LevelSection[i].Level;
		Var->LevelSection[i].FP = Var->LevelSection[i].P * Dis_Face;
		Var->LevelSection[i].Mp = Var->LevelSection[i].FP * Var->LevelSection[i].L_Y;
	}
	Var->LevelSection.end()->FP = 0.0;
	Var->LevelSection.end()->Mp = 0.0;

	//- Sum Total Wave Force
	Var->Fp = 0.0;
	Var->Mp = 0.0;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		Var->Fp += Var->LevelSection[i].FP;
		Var->Mp += Var->LevelSection[i].Mp;
	}
	Var->Err_Msg += "波壓計算處理完畢! \r\n";

	// Left Force
	Var->Pu = 0.5 * (1.0 + std::cos(Var->beta)) * Var->alpha1 * Var->alpha3 * Var->DensitySea * Var->Hmax * Var->lamda;

	Var->Fu = 0.5 * Var->Pu * Var->B;

	Var->Mu = (2.0 / 3.0) * Var->Fu * Var->B;

	Var->Err_Msg += "上揚力計算處理完畢! \r\n";

	return true;
}

bool Module1_Internal::WeightCal()
{
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->BlockData[i].SelfWeight = Var->BlockData[i].Area * Var->BlockData[i].Density;
		Var->BlockData[i].Mw = Var->BlockData[i].SelfWeight * Var->BlockData[i].WeightC.x;
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
	return false;
}
