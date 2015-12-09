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
	Var->Err_Msg += "Finish GeoPreCal() \n";
	return true;
}

bool Module1_Internal::WaterLevelCal()
{
	Var->L0 = 1.56 * Var->T0 * Var->T0;
	Var->H0_plun = Var->H0 * Var->Kr * Var->Kd;
	Var->h_D_L0 = Var->h / Var->L0;
	//L???
	
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
	}
	else {
		Var->Hs = std::min(std::min(Var->beta0 * Var->H0_plun + Var->beta1 * Var->h, Var->betaMax * Var->H0_plun), KsHo_p);
	}
	return true;
}
