// 這是主要 DLL 檔案。

#include "Module2.h"

VE_SD::Module2::Module2():Var(NULL), Internal(NULL)
{
	//Initial Obj
	Var = new Module2_Var();
	Internal = new Module2_Internal();
	//Get Variable
	Internal->SetVar(Var);

	//Mesg
	ErrMsg += "*** Module - 2 計算開始 *** \r\n";
}

VE_SD::Module2::~Module2()
{
	
}

VE_SD::Module2::!Module2()
{
	this->~Module2(); 
}

void VE_SD::Module2::WaterDesignInput(double _HWL, double _LWL)
{
	Var->HWL = _HWL;
	Var->LWL = _LWL;
}

void VE_SD::Module2::ForceDesignInput(double _Q, double _Qe, double _Ta)
{
	Var->Q = _Q;
	Var->Qe = _Qe;
	Var->Ta = _Ta;
}

void VE_SD::Module2::EarthquakeDesignInput(double _K, double _K_plun)
{
	Var->K = _K;
	Var->K_plun = _K_plun;
}

void VE_SD::Module2::MaterialDesignInput(double _InnerPhi, double _WallPhi, double _Beta)
{
	Var->InnerPhi = _InnerPhi;
	Var->WallPhi = _WallPhi;
	Var->Beta = _Beta;
}

void VE_SD::Module2::BaseDesignInput(double _U, double _D, double _BasePhi, double _C)
{
	Var->U = _U;
	Var->D = _D;
	Var->BasePhi = _BasePhi;
	Var->C = _C;
}

void VE_SD::Module2::MF_DesignInput(double _Nq, double _Nr, double _Nc)
{
	Var->Nq = _Nq;
	Var->Nr = _Nr;
	Var->Nc = _Nc;
}

void VE_SD::Module2::SF_CoefInput(double _SlideSF, double _RotateSF, double _BaseSF)
{
	Var->SlideSF = _SlideSF;
	Var->RotateSF = _RotateSF;
	Var->BaseSF = _BaseSF;
}
