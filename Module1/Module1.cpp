// �o�O�D�n DLL �ɮסC

#include "stdafx.h"
#include "Module1.h"
#include <fstream>

VE_SD::Module1::Module1():Var(NULL),Internal(NULL)
{
	//Initial Obj
	Var = new Module1_Var();
	Internal = new Module1_Internal();
	//Get Variable
	Internal->SetVar(Var);

	//Mesg
	ErrMsg += "*** Module - 1 �p��}�l *** \r\n";
}

VE_SD::Module1::~Module1()
{
	if (Internal != NULL) delete Internal;
	if (Var != NULL) delete Var;

}

VE_SD::Module1::!Module1()
{
	this->~Module1();
}

int VE_SD::Module1::NewBlock(double _Density, double _FrictionC){
	Var->BlockData.emplace_back(_Density,_FrictionC);
	return int(Var->BlockData.size());
}

bool VE_SD::Module1::DeleteBlock(int NumOfBlock){
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

bool VE_SD::Module1::SetBlockCoord(int NumOfBlock, double x, double y){
	if(Var->BlockData.size()!=0 && NumOfBlock <= int(Var->BlockData.size())){
		int Id = NumOfBlock - 1;
		Var->BlockData[Id].Node.emplace_back(x, y);
		return true;
	}
	else {
		ErrMsg += "Wrong Delete Block Id (In Set Block Coord)! \n";
		return false;
	}
		
}

int VE_SD::Module1::GetNumOfBlock()
{
	return int(Var->BlockData.size());
}

bool VE_SD::Module1::DeleteAllBlockData()
{
	Var->BlockData.clear();
	if (GetNumOfBlock() == 0) {
		return true;
	}
	else {
		return false;
	}
	
}

bool VE_SD::Module1::WaterDesignInput(double _H0, double _HWL, double _DensitySea)
{
	Var->H0 = _H0;
	Var->HWL = _HWL;
	Var->DensitySea = _DensitySea;
	return true;
}

bool VE_SD::Module1::WaveDesignInput(int _Direction, double _T0, double _Kr, 
									double _Ks, double _Kd, double _lamda,
									double _beta)
{
	Var->Direction = _Direction;
	Var->T0 = _T0;
	Var->Kr = _Kr;
	Var->Ks = _Ks;
	Var->Kd = _Kd;
	Var->lamda = _lamda;
	Var->beta = _beta;
	return true;
}

bool VE_SD::Module1::BaseDesignInput(double _S)
{
	Var->S = _S;
	return true;
}

bool VE_SD::Module1::Run()
{
	// Geo Pre-Calculate
	Internal->GeoPreCal();
	// Water Level Cal
	Internal->WaterLevelCal();
	//// Wave Pressure Moment Cal
	//Internal->WavePressureCal();
	//// Self Weight Moment Cal
	//Internal->WeightCal();

	//// Mesg Print
	//MsgAdd();

	//Mesg
	ErrMsg += "*** Module - 1 �p�⵲�� *** \r\n";

	
	//Test---

	return true;
}

bool VE_SD::Module1::OutPutLogFile(String ^ Pois)
{
	//- File Open
	std::ofstream FILE;
	std::string C_str = msclr::interop::marshal_as<std::string>(Pois);
	FILE.open(C_str);
	//- Out Contents
	FILE << "******�I���Ѽ�******"<< std::endl;
	FILE << "�Y�V: " << Var->Direction << std::endl;
	FILE << "�i��: " << Var->H0 << std::endl;
	FILE << "�g��: " << Var->T0 << std::endl;
	FILE << "���: " << Var->HWL << std::endl;
	FILE << "�Y��: " << Var->S << std::endl;
	FILE << "��g�Y��: " << Var->Kr << std::endl;
	FILE << "�L�ƫY��: " << Var->Ks << std::endl;
	FILE << "¶�g�Y��: " << Var->Kd << std::endl;
	FILE << "���Y��: " << Var->lamda << std::endl;
	FILE << "���u����: " << Var->beta << std::endl;
	FILE << "�����K��: " << Var->DensitySea << std::endl;
	FILE << "******�X��϶�******" << std::endl;
	FILE << "�`�϶���: " << Var->BlockData.size() << std::endl;
	FILE <<  std::endl;
	for (size_t i = 0; i < Var->BlockData.size(); i++)
	{
		FILE << "�϶��椸 " << i+1 <<" :"<< std::endl;
		FILE << "�϶��K��: " << Var->BlockData[i].Density << std::endl;
		FILE << "�϶�����: " << Var->BlockData[i].FrictionC << std::endl;
		FILE << "----�`�I�y��----" << std::endl;
		for (auto & NodeElement : Var->BlockData[i].Node) {
			FILE << "X: " << NodeElement.x << "\t" << "Y: " << NodeElement.y << std::endl;
		}
		FILE << std::endl;
	}
	FILE.close();
	return true;
}

void VE_SD::Module1::Test()
{
	Internal->TestFileOut("Density.txt", Var->BlockData[0].Density);
	Internal->TestFileOut("FrictionC.txt", Var->BlockData[0].FrictionC);

	std::ofstream FILE;
	FILE.open("Coord.txt");
	for (auto Block : Var->BlockData) {
		FILE << "Block" << std::endl;
		for (auto Node : Block.Node) {
			FILE << Node.x << "\t" << Node.y << std::endl;
		}
	}

	FILE.close();
}

void VE_SD::Module1::VarOut(double % out)
{
	out = Var->beta;
}

void VE_SD::Module1::MsgAdd()
{
	ErrMsg += gcnew String(Var->Err_Msg.c_str());
	Var->Err_Msg.clear();
}
