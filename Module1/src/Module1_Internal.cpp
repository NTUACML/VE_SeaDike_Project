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

	//Get Max and Min level of All Block
	Var->Max_level = Var->Ref_y;
	Var->Min_level = Var->Ref_y;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= Var->Min_level) Var->Min_level = Var->BlockData[i].MinLevel;
		if (Var->BlockData[i].MaxLevel >= Var->Max_level) Var->Max_level = Var->BlockData[i].MaxLevel;
	}

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

	Var->Err_Msg += "�p��X��e�B�z����! \r\n";
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

	Var->Err_Msg += "- ���`����p�⦨�\! \r\n";

	// Cal definetion of water level

	Var->L0 = 1.56 * Var->T0 * Var->T0;

	Var->H0_plun = Var->H0 * Var->Kr * Var->Kd;

	Var->h_D_L0 = Var->h / Var->L0;

	// Dispersion Relation Start
	Var->Err_Msg += "- �������Y���p��}�l! \r\n";

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
			Var->Err_Msg += "- �������Y���p�⦨�\! \r\n";
			Var->L = 2.0 * M_PI * Var->h / kh;
			break;
		}
		
		kh_1 = kh;	
		++count;	
	}

	if (count >= Max_Itr) {
		Var->L = 2.0 * M_PI * Var->h / kh;
		Var->Err_Msg += "- �������Y���p�⥢��! \r\n";
		Var->Err_Msg += "- �T�O�ƭȺ�T!�{������p��! \r\n";
		return false;
	}
	
	
	Var->beta0 = 0.028 *std::pow(Var->H0_plun / Var->L0, -0.38) * std::exp(20.0 * std::pow(Var->S, 1.5));

	Var->beta1 = 0.52 * std::exp(4.2 * Var->S);

	Var->betaMax = std::max(0.92,
					0.32 *std::pow(Var->H0_plun / Var->L0, -0.29) * std::exp(2.4 * Var->S));

	Var->beta0_Star = 0.052 *std::pow(Var->H0_plun / Var->L0, -0.38) * std::exp(20.0 * std::pow(Var->S, 1.5));

	Var->beta1_Star = 0.63 * std::exp(3.8 * Var->S);

	Var->betaMax_Star = std::max(1.65,
		0.53 *std::pow(Var->H0_plun / Var->L0, -0.29) * std::exp(2.4 * Var->S));

	double KsHo_p;

	KsHo_p = Var->Ks * Var->H0_plun;

	if (Var->h_D_L0 >= 0.2) {
		Var->Hs = KsHo_p;
	}
	else {
		Var->Hs = std::min(std::min(Var->beta0 * Var->H0_plun + Var->beta1 * Var->h, Var->betaMax * Var->H0_plun), KsHo_p);
	}

	if (Var->hb < -9998) {
		Var->hb = Var->h + 5.0 * Var->Hs * Var->S;
	}
	
	if (Var->h_D_L0 >= 0.2) {
		Var->Hmax = 1.8 * KsHo_p;
	}
	else {
		Var->Hmax = std::min(std::min(Var->beta0_Star * Var->H0_plun + Var->beta1_Star * Var->hb, Var->betaMax_Star * Var->H0_plun), 1.8*KsHo_p);
	}

	Var->Err_Msg += "�I�����z��ƳB�z����! \r\n";
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

	Var->P2 = Var->P1 / std::cosh(2.0 * M_PI * Var->h / Var->L) ;

	Var->P3 = Var->alpha3 * Var->P1;

	Var->P4 = Var->P1 * Var->alpha4;
	// Pressure Moment.
	//- Find HWL 
	size_t HWL_ID = 0;
	double eps = 1e-3, Dis_Face, M;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		if ((Var->LevelSection[i].Level) > (Var->HWL - eps)
			&&
			(Var->LevelSection[i].Level) < (Var->HWL + eps)
			) {
			HWL_ID = i;
		}
	}
	//- Put Wave P magnitude
	//-- Put P3
	Var->LevelSection.begin()->P = Var->P3;
	//-- Put P4
	(Var->LevelSection.end()-1)->P = Var->P4;
	//-- Put HWL
	Var->LevelSection[HWL_ID].P = Var->P1;
	//-- Interpolation
	M = (Var->LevelSection[HWL_ID].P - Var->LevelSection.begin()->P) / (Var->LevelSection[HWL_ID].Level - Var->LevelSection.begin()->Level);
	for (size_t i = 1; i < HWL_ID; i++)
	{
		Var->LevelSection[i].P = Var->LevelSection.begin()->P + M * (Var->LevelSection[i].Level - Var->LevelSection.begin()->Level);
	}
	M = ((Var->LevelSection.end()-1)->P - Var->LevelSection[HWL_ID].P) / ((Var->LevelSection.end()-1)->Level - Var->LevelSection[HWL_ID].Level);
	for (size_t i = HWL_ID + 1; i < Var->LevelSection.size()-1 ; i++)
	{
		Var->LevelSection[i].P = Var->LevelSection[HWL_ID].P + M * (Var->LevelSection[i].Level - Var->LevelSection[HWL_ID].Level);
	}
	//- Wave Pressure & Moment
	Var->LevelSection.begin()->FP = 0.0;
	Var->LevelSection.begin()->L_Y = 0.0;
	Var->LevelSection.begin()->Mp = 0.0;


	double F_Sum = 0.0;
	for (size_t i = 1; i < Var->LevelSection.size(); i++)
	{
		Dis_Face = Var->LevelSection[i].Level - Var->LevelSection[i-1].Level;
		Var->LevelSection[i].FP = (Var->LevelSection[i].P + Var->LevelSection[i-1].P)  * Dis_Face / 2.0;

		// Tragngle Used
		F_Sum = Var->LevelSection[i - 1].P * Dis_Face * (Dis_Face / 2.0) + (Var->LevelSection[i].P - Var->LevelSection[i - 1].P) * (Dis_Face / 2.0) * (2.0 * Dis_Face / 3.0);
		
		Var->LevelSection[i].L_Y = (Var->LevelSection[i - 1].Level - Var->LevelSection.begin()->Level) + F_Sum / Var->LevelSection[i].FP;

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
	Var->Err_Msg += "�i���p��B�z����! \r\n";

	// Left Force
	Var->Pu = 0.5 * (1.0 + std::cos(Var->beta)) * Var->alpha1 * Var->alpha3 * Var->DensitySea * Var->Hmax * Var->lamda;

	Var->Fu = 0.5 * Var->Pu * Var->B;

	Var->Mu = (2.0 / 3.0) * Var->Fu * Var->B;

	Var->Err_Msg += "�W���O�p��B�z����! \r\n";

	return true;
}

bool Module1_Internal::WeightCal()
{
	double eps = 1e-3;
	// Find Ref x with different direction
	double Ref_x = Var->Ref_x;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->Direction == 1) {
			if(Var->BlockData[i].MinX <= Ref_x)
				Ref_x = Var->BlockData[i].MinX;
		}
		else {
			if (Var->BlockData[i].MaxX >= Ref_x)
				Ref_x = Var->BlockData[i].MaxX;
		}
	}

	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->BlockData[i].SelfWeight = Var->BlockData[i].Area * Var->BlockData[i].Density;
		if (Var->BlockData[i].CalMoment == false) //Don't cal Moment.
		{
			Var->BlockData[i].X = 0.0;
			Var->BlockData[i].Mw = 0.0; 
		}
		else {
			Var->BlockData[i].X = std::abs(Var->BlockData[i].WeightC.x - Ref_x);
			Var->BlockData[i].Mw = Var->BlockData[i].SelfWeight * Var->BlockData[i].X;
		}	
	}
	//- Sum Total Weight and Moment
	Var->W = 0.0;
	Var->Mw = 0.0;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		Var->W += Var->BlockData[i].SelfWeight;
		Var->Mw += Var->BlockData[i].Mw;
	}

	Var->Err_Msg += "����ۭ��O�p��B�z����! \r\n";
	return true;
}

bool Module1_Internal::BodySafeCheck()
{
	//Find Min Weight_Y
	double MinWeight_Y = Var->Ref_y;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].WeightC.y <= MinWeight_Y) MinWeight_Y = Var->BlockData[i].WeightC.y;
	}
	//Get Down Block average Mu coef
	int Count = 0;
	double AveMu = 0.0;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		if (Var->BlockData[i].MinLevel <= MinWeight_Y) {
			AveMu += Var->BlockData[i].FrictionC;
			++Count;
		}
	}
	AveMu /= double(Count);

	Var->CalBody_SlideSF = AveMu * (Var->W - Var->Fu) / Var->Fp;

	Var->CalBody_RotateSF = (Var->Mw - Var->Mu) / Var->Mp;

	if (Var->CalBody_SlideSF >= Var->SlideSF) {
		Var->Err_Msg += "�ư��ˮ� (�q�L)! \r\n";
	}
	else {
		Var->Err_Msg += "�ư��ˮ� (����)! \r\n";
	}

	if (Var->CalBody_RotateSF >= Var->RotateSF) {
		Var->Err_Msg += "�ɭ��ˮ� (�q�L)! \r\n";
	}
	else {
		Var->Err_Msg += "�ɭ��ˮ� (����)! \r\n";
	}

	return true;
}

bool Module1_Internal::BreakerSafeCheck()
{
	double Sr, CotTheta;
	if (Var->WaveBreakFuncOutside == true) {
		Sr = Var->DensityOutside / Var->DensitySea;
		CotTheta = 1.0 / std::tan(Var->SlopeOutside * M_PI / 180.0);
		Var->W1 = (Var->DensityOutside * std::pow(Var->Hs, 3.0)) /
			(Var->SafeCoefOutside * std::pow(Sr - 1.0, 3.0) * CotTheta);
		Var->Err_Msg += "���i�u�{-��~���p�⧹��! \r\n";
	}

	if (Var->WaveBreakFuncUpside == true) {
		Sr = Var->DensityUpside / Var->DensitySea;
		CotTheta = 1.0 / std::tan(Var->SlopeUpside * M_PI / 180.0);
		Var->W2 = 1.5 * (Var->DensityUpside * std::pow(Var->Hs, 3.0)) /
			(Var->SafeCoefUpside * std::pow(Sr - 1.0, 3.0) * CotTheta);
		Var->Err_Msg += "���i�u�{-���Y���[�j�p�⧹��! \r\n";
	}

	if (Var->WaveBreakFuncInside == true) {
		double h_plun_D_h, hc_D_Hs, Ht, Sr;
		Sr = Var->DensityInside / Var->DensitySea;
		h_plun_D_h = Var->h_plun / Var->h;
		hc_D_Hs = Var->hc / Var->Hs;
		Ht = Var->Kt * Var->Hs;

		Var->W3 = (Var->DensityInside * std::pow(Ht, 3.0)) /
			(Var->SafeCoefInside * std::pow(Sr - 1.0, 3.0) * CotTheta);

		Var->Err_Msg += "���i�u�{-�������[�j�p�⧹��! \r\n";
	}
	return true;
}

bool Module1_Internal::UpperSafeCheck()
{
	if (Var->UpperBlockCheckCondi) {
		std::vector<size_t> UpperBlock, UpperLevelSec;
		double SumFP = 0.0, SumBlockMuTimesWc = 0.0;

		// Find Upper Block Id
		for (size_t i = 0; i < Var->BlockData.size(); ++i) {
			if (Var->BlockData[i].WeightC.y >= Var->UpBlockEL)
				UpperBlock.push_back(i);
		}

		// Find Upper Level Section
		for (size_t i = 0; i < Var->LevelSection.size(); ++i) {
			if (Var->LevelSection[i].Level > Var->UpBlockEL)
				UpperLevelSec.push_back(i);
		}

		// Cal Sum Fp
		for (auto id : UpperLevelSec) {
			SumFP += Var->LevelSection[id].FP;
		}

		// Cal Sum Block Mu * Wc
		for (auto id : UpperBlock) {
			SumBlockMuTimesWc += Var->BlockData[id].FrictionC * Var->BlockData[id].SelfWeight;
		}

		// Cal SlideSF
		Var->CalUpper_SlideSF = SumBlockMuTimesWc / SumFP;

		// Check Slide
		if (Var->CalUpper_SlideSF >= Var->SlideSF) {
			Var->Err_Msg += "���𳡷ư��ˮ� (���\)! \r\n";
		}
		else {
			Var->Err_Msg += "���𳡷ư��ˮ� (����) -> �ҰʰŤO�g�p��! \r\n";
			Var->Bk = ((1.2 * SumFP - SumBlockMuTimesWc) / Var->Vc) * 100.0;
			Var->Err_Msg += "�ŤO�g�p�⧹��! \r\n";

			if (Var->Bk_plun >= Var->Bk) {
				Var->Err_Msg += "�]�p���ŤO�g (����) �p��һ�! \r\n";
			}
			else {
				Var->Err_Msg += "�]�p���ŤO�g (������) �p��һ�! \r\n";
			}
		}

		// Cal Ref Pt
		double Rf_x = Var->Ref_x, Rf_y = Var->Ref_y;
		double Upper_Mw = 0.0, Upper_Mp = 0.0;
		Rf_x = Var->BlockData[*UpperBlock.begin()].WeightC.x;
		Rf_y = Var->BlockData[*UpperBlock.begin()].WeightC.y;
		if (Var->Direction == 1) { // E direction (Find Min Node)	
			for (auto id : UpperBlock) {
				for (auto node : Var->BlockData[id].Node) {
					if (node.x <= Rf_x)
						Rf_x = node.x;
					if (node.y <= Rf_y)
						Rf_y = node.y;
				}
			}
		}
		else{ // W direction (Find Max Node)	
			for (auto id : UpperBlock) {
				for (auto node : Var->BlockData[id].Node) {
					if (node.x >= Rf_x)
						Rf_x = node.x;
					if (node.y <= Rf_y)
						Rf_y = node.y;
				}
			}
		}

		// Cal Upper_Mw
		for (auto id : UpperBlock) {
			Upper_Mw += Var->BlockData[id].SelfWeight * std::abs(Var->BlockData[id].WeightC.x - Rf_x);
		}

		// Cal Upper_Mp
		double CorrectY_Length = std::abs(Rf_y - Var->LevelSection.begin()->Level);
		for (auto id : UpperLevelSec) {
			Upper_Mp += Var->LevelSection[id].FP * std::abs(Var->LevelSection[id].L_Y - CorrectY_Length);
		}

		// Cal RotateS
		Var->CalUpper_RotateSF = Upper_Mw / Upper_Mp;

		// Check Rotate
		if (Var->CalUpper_RotateSF >= Var->RotateSF) {
			Var->Err_Msg += "���𳡶ɭ��ˮ� (���\)! \r\n";
		}
		else {
			Var->Err_Msg += "���𳡶ɭ��ˮ� (����)! \r\n";
		}

	}
	
	return true;
}

bool Module1_Internal::BasementSafeCheck()
{
	if (Var->BasementCheckCondi) {
		// �@�ΤO
		Var->V = Var->W - Var->Fu;
		Var->H = Var->Fp;
		Var->Mr = Var->Mw - Var->Mu;
		Var->Mo = Var->Mp;

		// �������ϤO
		Var->B_6 = Var->B / 6.0;
		Var->C_x = (Var->Mr - Var->Mo) / Var->V;
		Var->e_x = (Var->B / 2.0) - Var->C_x;
		
		if (Var->e_x <= Var->B_6) {
			Var->B_plum = Var->B;
			Var->Base_P1 = (1.0 + (6.0 * Var->e_x) / Var->B) * (Var->V / Var->B);
			Var->Base_P2 = (1.0 - (6.0 * Var->e_x) / Var->B) * (Var->V / Var->B);
		}
		else {
			Var->B_plum = 3.0 * Var->C_x;
			Var->Base_P1 = (2.0 * Var->V) / (3.0 * Var->C_x);
			Var->Base_P2 = 0.0;
		}

		// ��¦�ߥ۩����ϤO
		Var->Df = Var->U + Var->D;
		Var->Base_Theta = atan(Var->H / Var->V); // (Rad)
		Var->B_plum2 = Var->B_plum + Var->D * (tan(M_PI / 6.0 + Var->Base_Theta) + tan(M_PI / 6.0 - Var->Base_Theta));
		double DiffB = (Var->B_plum / Var->B_plum2) ;
		Var->R1 = Var->Base_P1 * DiffB + Var->BaseDen * Var->D;
		Var->R2 = Var->Base_P2 * DiffB + Var->BaseDen * Var->D;

		// �a�L�����O
		Var->Qu = Var->C * Var->Nc + Var->BaseDen * Var->Df * Var->Nq + 0.5 * Var->BaseDen * Var->B_plum2 * Var->Nr;
		Var->Qa = Var->Qu / Var->BaseFS + Var->BaseDen * Var->Df;

		// Check Rotate
		if (Var->Qa >= Var->R1) {
			Var->Err_Msg += "�a�L�����O�ˮ� (���\)! \r\n";
		}
		else {
			Var->Err_Msg += "�a�L�����O�ˮ� (����)! \r\n";
		}

		// (Rad) -> (degree)
		Var->Base_Theta *= (180.0 / M_PI);
		Var->CentAngle *= (180.0 / M_PI);
		
	}
	return true;
}

