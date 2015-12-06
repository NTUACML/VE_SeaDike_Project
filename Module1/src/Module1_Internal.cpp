#include "../include/Module1_Internal.h"

Module1_Internal::Module1_Internal():Gv(NULL)
{
}

Module1_Internal::~Module1_Internal()
{
}

void Module1_Internal::SetVar(Module1_Var * Var)
{
	Gv = Var;
	std::ofstream FILE;
	FILE.open("Haha.txt");
	FILE << "HAHAHA" << std::endl;
	FILE.close();
}
