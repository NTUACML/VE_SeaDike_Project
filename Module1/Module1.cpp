// 這是主要 DLL 檔案。

#include "stdafx.h"
#include "Module1.h"

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
	Var->BlockData.emplace_back();
	return int(Var->BlockData.size());
}

bool VE_SD::Module1::DeleteBlock(int NumOfBlock){
	if(Var->BlockData.size()!=0 && int(Var->BlockData.size())<=NumOfBlock){
		int Id = NumOfBlock - 1;
		Var->BlockData.erase(Itr + Id);
		return true;
	}
	else
		return false;
}

bool VE_SD::Module1::SetBlockCoord(int NumofBlock, double x, double y){
	if(Var->BlockData.size()!=0 && int(Var->BlockData.size())<=NumOfBlock){
		int Id = NumOfBlock - 1;
		Var->BlockData[Id].Node.emplace_back(x, y);
		return true;
	}
	else
		return false;
}
