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
	Var->Err_Msg += "- 未完成 GeoPreCal() \r\n";

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
