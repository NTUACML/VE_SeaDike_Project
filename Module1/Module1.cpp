// 這是主要 DLL 檔案。

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
}

VE_SD::Module1::~Module1()
{
	if (Internal != NULL) delete Internal;
	if (Var != NULL) delete Var;
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
