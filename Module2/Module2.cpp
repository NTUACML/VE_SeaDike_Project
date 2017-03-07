// �o�O�D�n DLL �ɮסC

#include "Module2.h"

VE_SD::Module2::Module2():Var(NULL), Internal(NULL)
{
	//Initial Obj
	Var = new Module2_Var();
	Internal = new Module2_Internal();
	//Get Variable
	Internal->SetVar(Var);

	//Mesg
	ErrMsg += "*** Module - 2 �p��}�l *** \r\n";
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

void VE_SD::Module2::WaterDesignInput(double _HWL, double _LWL, double _RWL)
{
	Var->HWL = _HWL;
	Var->LWL = _LWL;
	Var->RWL = _RWL;
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

void VE_SD::Module2::BaseDesignInput(double _U, double _D, double _BasePhi, double _C, double _soilR_Earth, double _soilR_Water)
{
	Var->U = _U;
	Var->D = _D;
	Var->BasePhi = _BasePhi;
	Var->C = _C;
	Var->soilR_Earth = _soilR_Earth;
	Var->soilR_Water = _soilR_Water;
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
//�s�W�a�_�ɦw���Y��
void VE_SD::Module2::SF_CoefInput_E(double _SlideSF_E, double _RotateSF_E, double _BaseSF_E)
{
	Var->SlideSF_E = _SlideSF_E;
	Var->RotateSF_E = _RotateSF_E;
	Var->BaseSF_E = _BaseSF_E;
}

void VE_SD::Module2::KaInput(double _ka, double _ka_17, double _ka_33)
{
	Var->ka = _ka;
	Var->ka_17 = _ka_17;
	Var->ka_33 = _ka_33;
}

int VE_SD::Module2::NewBlock(double _Density, double _EQ_Density, double _FrictionC, bool _CalMoment) {
	Var->BlockData.emplace_back(_Density, _EQ_Density, _FrictionC, _CalMoment);
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
	//Internal->EarthQuakeForceCal();
	Internal->HorizontalSoilForceCal();
	Internal->VertivalSoilForceCal();

	return true;
}

bool VE_SD::Module2::OutPutLogFile(String ^ Pois)
{
	// File Open
	std::ofstream FILE;
	std::string C_str = msclr::interop::marshal_as<std::string>(Pois);
	FILE.open(C_str);

	// File Contain
	FILE << "******�I���Ѽ�******" << std::endl;
	FILE << "HWL: " << Var->HWL << std::endl;
	FILE << "LWL: " << Var->LWL << std::endl;
	FILE << "Ka: " << Var->ka << std::endl;

	FILE << std::endl;
	FILE << "******�����Ѽ�******" << std::endl;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		//Var->BlockData[i].Cal_Area();
		
		FILE << "�϶��椸 " << i + 1 << " :" << std::endl;
		FILE << "�϶��K��: " << Var->BlockData[i].Density << std::endl;
		FILE << "�϶�����: " << Var->BlockData[i].FrictionC << std::endl;
		FILE << "������n: " << Var->BlockData[i].Area << std::endl;
		FILE << "----�`�I�y��----" << std::endl;
		for (auto & NodeElement : Var->BlockData[i].Node) {
			FILE << "X: " << NodeElement.x << "\t" << "Y: " << NodeElement.y << std::endl;
		}
		FILE << std::endl;
		//Var->BlockData[i].SelfWeight = Var->BlockData[i].Area*Var->BlockData[i].Density;
		FILE << "----����ۭ��P�O�x----" << std::endl;
		FILE << "SelfWeight:" << Var->BlockData[i].SelfWeight << std::endl;
		FILE << "X:" << Var->BlockData[i].X << std::endl;
		FILE << "Mw:" << Var->BlockData[i].Mw << std::endl;
		FILE << "minix:" << Var->BlockData[i].MinX << std::endl;
		FILE << std::endl;
	}

	FILE << std::endl;
	FILE << "******Level�Ѽ�******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL(" << Var->LevelSection[i].Level << ")" << i + 1 << " :";
		for (size_t j = 0; j < Var->LevelSection[i].BlockId.size(); j++)
		{
			FILE << Var->LevelSection[i].BlockId[j]+1 << " ";
		}
		FILE << std::endl;
	}
	//FILE << "******Level�Ѽ�-���`�p******" << std::endl;
	//for (size_t i = 0; i < Var->LevelSection.size(); i++)
	//{
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "����ۭ�-�p: " << Var->LevelSection[i].Level_sum_W << std::endl;
	//	FILE << "�O�u-�p: " << Var->LevelSection[i].Level_total_arm << std::endl;
	//	FILE << "����s�x-�p: " << Var->LevelSection[i].Level_sum_Mx << std::endl;
	//}

	//FILE << "******Level�Ѽ�-�e�`�p******" << std::endl;
	//for (size_t i = 1; i < Var->LevelSection.size(); i++)
	//{
	//	FILE << "EL" << i + 1 << " :" << std::endl;
	//	FILE << "����ۭ�-�p: " << Var->LevelSection[i].pre_sum_W << std::endl;
	//	FILE << "�O�u-�p: " << Var->LevelSection[i].pre_total_arm << std::endl;
	//	FILE << "����s�x-�p: " << Var->LevelSection[i].pre_sum_Mx << std::endl;
	//}

	/*FILE << "******Earthquake Level�Ѽ�-���`�p******" << std::endl;
	for (size_t i = 0; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "����ۭ�-�p: " << Var->LevelSection[i].Level_sum_WE << std::endl;
		FILE << "�O�u-�p: " << Var->LevelSection[i].Level_total_armE << std::endl;
		FILE << "����s�x-�p: " << Var->LevelSection[i].Level_sum_MxE << std::endl;
	}

	FILE << "******Earthquake Level�Ѽ�-�e�`�p******" << std::endl;
	for (size_t i = 1; i < Var->LevelSection.size(); i++)
	{
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "����ۭ�-�p: " << Var->LevelSection[i].pre_sum_WE << std::endl;
		FILE << "�O�u-�p: " << Var->LevelSection[i].pre_total_armE << std::endl;
		FILE << "����s�x-�p: " << Var->LevelSection[i].pre_sum_MxE << std::endl;
	}*/

	//- ���|
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "�������O: " << Var->LevelSection[i].Fh << std::endl;
		FILE << "�O�x: " << Var->LevelSection[i].Fh_y << std::endl;
		FILE << "�ɭ��s�x: " << Var->LevelSection[i].Fh_Mh << std::endl;
	}
	FILE << std::endl;
	//- ��椭
	for (size_t i = 0; i < Var->LevelSection.size(); i++) {
		FILE << "EL" << i + 1 << " :" << std::endl;
		FILE << "�������O: " << Var->LevelSection[i].Fv_sum << std::endl;
		FILE << "�O�x: " << Var->LevelSection[i].Fv_x << std::endl;
		FILE << "����s�x: " << Var->LevelSection[i].Fv_Mv_sum << std::endl;
	}

	// File Close
	FILE.close();
	return true;
}
