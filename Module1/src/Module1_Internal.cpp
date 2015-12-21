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

	Var->Err_Msg += "計算幾何前處理完畢! \r\n";
	return true;
}

bool Module1_Internal::WaterLevelCal()
{
	Var->L0 = 1.56 * Var->T0 * Var->T0;
	Var->H0_plun = Var->H0 * Var->Kr * Var->Kd;
	Var->h_D_L0 = Var->h / Var->L0;
	//L???
	Var->Err_Msg += "- 延散關係式未完成! \r\n";
	
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

	// Pressure Moment.
	Var->Err_Msg += "- 傾倒力矩未完成! \r\n";


	Var->Err_Msg += "波壓計算處理完畢! \r\n";
	return true;
}
