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
	if (Internal != NULL) delete Internal;
	if (Var != NULL) delete Var;
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
//新增地震時安全係數
void VE_SD::Module2::SF_CoefInput_E(double _SlideSF_E, double _RotateSF_E, double _BaseSF_E)
{
	Var->SlideSF_E = _SlideSF_E;
	Var->RotateSF_E = _RotateSF_E;
	Var->BaseSF_E = _BaseSF_E;
}

int VE_SD::Module2::NewBlock(double _Density, double _FrictionC, bool _CalMoment) {
	Var->BlockData.emplace_back(_Density, _FrictionC, _CalMoment);
	return int(Var->BlockData.size());
}

bool VE_SD::Module2::DeleteBlock(int NumOfBlock) {
	if (Var->BlockData.size() != 0 && NumOfBlock <= int(Var->BlockData.size())) {
		int Id = NumOfBlock - 1;
		Var->BlockData.erase(Var->BlockData.begin() + Id);
		return true;
	}
	else {
		ErrMsg += ("Wrong Delete Block Id !(In Delete Block " + NumOfBlock.ToString() + ") \n");
		return false;
	}
}

bool VE_SD::Module2::SetBlockCoord(int NumOfBlock, double x, double y) {
	if (Var->BlockData.size() != 0 && NumOfBlock <= int(Var->BlockData.size())) {
		int Id = NumOfBlock - 1;
		Var->BlockData[Id].Node.emplace_back(x, y);
		return true;
	}
	else {
		ErrMsg += "Wrong Delete Block Id (In Set Block Coord)! \n";
		return false;
	}
}

int VE_SD::Module2::GetNumOfBlock()
{
	return int(Var->BlockData.size());
}

bool VE_SD::Module2::DeleteAllBlockData()
{
	Var->BlockData.clear();
	if (GetNumOfBlock() == 0) {
		return true;
	}
	else {
		return false;
	}
}

int VE_SD::Module2::NewLevel(double _EL)
{
	Var->LevelSection.emplace_back(_EL);
	return int(Var->LevelSection.size());
}

bool VE_SD::Module2::DeleteAllLevel()
{
	Var->LevelSection.clear();
	return true;
}

bool VE_SD::Module2::Run()
{
	Internal->GeoPreCal();
	Internal->WeightCal();

	return true;
}

bool VE_SD::Module2::OutPutLogFile(String ^ Pois)
{
	// File Open
	std::ofstream FILE;
	std::string C_str = msclr::interop::marshal_as<std::string>(Pois);
	FILE.open(C_str);

	// File Contain
	FILE << "******背景參數******" << std::endl;
	FILE << "HWL: " << Var->HWL << std::endl;
	FILE << "LWL: " << Var->LWL << std::endl;


	FILE << std::endl;
	FILE << "******型塊參數******" << std::endl;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		
		FILE << "區塊單元 " << i + 1 << " :" << std::endl;
		FILE << "區塊密度: " << Var->BlockData[i].Density << std::endl;
		FILE << "區塊摩擦: " << Var->BlockData[i].FrictionC << std::endl;
		FILE << "方塊面積: " << Var->BlockData[i].Area << std::endl;
		FILE << "----節點座標----" << std::endl;
		for (auto & NodeElement : Var->BlockData[i].Node) {
			FILE << "X: " << NodeElement.x << "\t" << "Y: " << NodeElement.y << std::endl;
		}
		FILE << std::endl;
		FILE << "----方塊自重與力矩----" << std::endl;
		FILE << "方塊自重:" << Var->BlockData[i].SelfWeight << std::endl;
		FILE << "力矩大小:" << Var->BlockData[i].Mw << std::endl;
		FILE << std::endl;
	}

	//FILE << "LWL: " << Var->LWL << std::endl;

	// File Close
	FILE.close();
	return true;
}
