// �o�O�D�n DLL �ɮסC

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
