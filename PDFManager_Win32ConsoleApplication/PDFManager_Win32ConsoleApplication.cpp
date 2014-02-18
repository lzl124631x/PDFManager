// PDFManager_Win32ConsoleApplication.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <Windows.h>
#define _CRT_SECURE_NO_WARNINGS //禁止fopen、fopen_s的warning
#define printUnicode(x); 	WriteConsoleW(GetStdHandle(STD_OUTPUT_HANDLE),x,(int)wcslen(x),NULL,NULL);

//链表节点
typedef struct LinkedListNode{
	void *data;
	struct LinkedListNode *next;
}*Node;

//链表
typedef struct LinkedList{
	Node head,rear;
	int length;
}*List,*CharList,*StringList,*SectionList,*XrefList;

//Xref片段
typedef struct _Section
{
	int startID;//起始编号
	int count;//条目数
	StringList Addrs;//地址链表
}*Section;

//文件尾
typedef struct _Trailer
{
	char *Prototype;//Trailer的字符串原型
	int Size;
	int Prev;
	int Root;
	int Info;
	char *ID[2];
	int Encrypt;
	int XRefStm;
}*Trailer;

//交叉引用片段
typedef struct _Xref
{
	int xrefOffset;//Xref的起始地址
	SectionList sections;//本Xref的所有子片段构成的链表
	Trailer trailer;//对应的文件尾
	int newPrev;//更新PDF时，此值保存了新的Prev
	int newXrefOffset;//更新PDF时，此值保存了新的Xref起始地址
}*Xref;

//属性的键值对
typedef struct _Attr
{
	LPSTR Name;
	LPWSTR Value;
}*Attr;

//初始化链表
List InitList()
{
	List list=(List)malloc(sizeof(LinkedList));
	Node headNode=(Node)malloc(sizeof(LinkedListNode));
	list->length=0;
	headNode->next=0;
	list->head=list->rear=headNode;
	return list;
}

//向链表中添加新的节点
void AddNode(List list, void *data)//AddNode(list,xref)传递的是引用，AddNode(list,(void*)'a')直接传值！
{
	Node p=(Node)malloc(sizeof(LinkedListNode));
	p->data=data;	
	p->next=0;
	list->rear->next=p;
	list->rear=p;
	list->length++;
}

//获取index处的节点数据
void *GetNodeData(List list, int index)
{
	int i;
	Node p=list->head->next;
	if(index>=list->length){
		printf("OutofBound Error!\n");
		exit(0);
	}
	for(i=0;i<index;i++){
		p=p->next;
	}
	return p->data;
}

//void DeleteNode(List list, int index)
//{
//	int i;
//	Node p=list->head,q=p->next;
//	if(index>=list->length){
//		printf("OutofBound Error!\n");
//		exit(0);
//	}
//	for(i=0;i<index;i++){
//		p=p->next;
//		q=p->next;
//	}
//	p->next=q->next;
//	free(q);
//	list->length--;
//}

//将CharList中保存所有字符组合成为一个字符串
char *CharListGetString(CharList list)//此处默认CharList中直接保存值！
{
	int i;
	Node p=list->head->next;
	char *String=(char*)malloc(sizeof(char)*(list->length+1));
	for(i=0;i<list->length;i++){
		String[i]=(char)p->data;
		p=p->next;
	}
	String[i]=0;
	return String;
}

//将StringList中保存的所有字符串组合成为一个字符串
LPSTR StringListGetString(StringList list)
{
	int i,j,k,size=0;
	LPSTR data;
	LPSTR String;
	for(i=0;i<list->length;i++){
		data=(char*)GetNodeData(list,i);
		if(data==0)continue;
		size+=(int)strlen(data);
	}
	String=(char*)malloc(sizeof(char)*(size+1));
	for(i=0,k=0;i<list->length;i++){
		data=(char*)GetNodeData(list,i);
		if(data==0)continue;
		for(j=0;data[j]!=0;j++)
		{
			String[k++]=data[j];
		}
	}
	String[k]=0;
	return String;
}

//从start位置截取字符串src的长度为len的子字符串
char *Substring(char *src, int start, int len)
{
	int srclen=(int)strlen(src),i;
	if(start+len>srclen)len=srclen-start;
	char *dest=(char*)malloc(sizeof(char)*(len+1));
	for(i=0;i<len;i++)
	{
		dest[i]=src[start+i];
	}
	dest[i]=0;
	return dest;
}

//尝试将字符串string转化为int值
//如果转化成功，将转化结果保存在value中，函数返回1
//否则函数返回0
int IntTryParse(char *string, int *value)
{
	int i,len=(int)strlen(string);
	*value=0;
	for(i=0;i<len;i++)
	{
		if(string[i]<48||string[i]>57)
		{
			*value=0;
			return 0;
		}
		*value*=10;
		*value+=string[i]-48;
	}
	return 1;
}

//确信字符串string能够转化为int值，函数直接返回转化结果
int IntParse(char *string)
{
	int i,value=0,len=(int)strlen(string);
	for(i=0;i<len;i++)
	{
		value*=10;
		value+=string[i]-48;
	}
	return value;
}

//PDF的字节流
static unsigned char *PDFInByte;
//文件大小
static int FileLength;
//该PDF文件是否能够被正常读取
static bool readable;
//Info对象的起始和结束地址
static int InfoAt;
static int InfoEnd;
//Tail对象的起始和结束地址
static int TailAt;
static int TailEnd;
//Tail对象编号
static int Tail;
//是否有Tail对象
static bool hasTail;
//所有Xref片段组成的链表，按照从新到旧的顺序排列
XrefList xrefs;

//最新的Trailer信息
int Size;
int Root;
int Info;
int Encrypt;
char *ID[2];

//PDF文件路径
char *PDFPath;
//第一个startxref值
int StartXref;
//PDF版本
char Version[4];
//Info信息
LPWSTR Title;
LPWSTR Author;
LPWSTR Subject;
LPWSTR Keywords;
LPWSTR CreationDate;
LPWSTR ModDate;
LPWSTR Creator;
LPWSTR Producer;
//自定义属性
List Attrs;
//隐藏属性
List Tails;

//获取文件大小
int getFileSize(char *File)
{
	FILE *fp=fopen(File,"rb");
	fseek(fp,0,SEEK_END);
	int size=ftell(fp);
	fclose(fp);
	return size;
}

//array为字符串，在它的index处向上（前）匹配长度为tLength的target字符串
int matchUp(unsigned char *array, int index, char *target,int tLength)
{
	int m = 1,i;
	for (i = 0; i < tLength && index - i >= 0 && m == 1; i++)
	{
		m = m && array[index - i] == target[tLength-1 - i];
	}
	return m;
}

//array为长度为length的字符串，从index处向上（前）查找target字符串
int findStringUp(unsigned char *array,int length, int index, char *target)
{
	int tLength=(int)strlen(target);
	if (index < 0) return -1;
	if (index >= length) index = length-1;
	while (!matchUp(array,index, target,tLength) && index >= 0)
	{
		index--;
	}
	return index;
}

//跳过空格、回车或换行
int omitBlank(unsigned char *array, int index)
{
	while (array[index] == 32 || array[index] == 13 || array[index] == 10) index++;
	return index;
}

//从array字符串的index处获取int值
int getNumber(unsigned char *array, int *index)
{
	int num = 0;
	while (array[*index] != 32 && array[*index] != 13 && array[*index] != 10)
	{
		num *= 10;
		num += array[(*index)++] - 48;//此处(*index)++的括号不能少！！！
	}
	return num;
}

//将代表十六进制数的num字符串转化为十进制值
int GetDecimal(char *num)
{
	int dec = 0,i,len=(int)strlen(num);
	for(i=0;i<len;i++)
	{
		dec *= 16;
		if (num[i] >= 48 && num[i] <= 57)
		{
			dec += num[i] - 48;
		}
		else
		{
			switch (num[i])
			{
			case 'A':
			case 'a': dec += 10; break;
			case 'B':
			case 'b': dec += 11; break;
			case 'C':
			case 'c': dec += 12; break;
			case 'D':
			case 'd': dec += 13; break;
			case 'E':
			case 'e': dec += 14; break;
			case 'F':
			case 'f': dec += 15; break;
			}
		}
	}
	return dec;
}

void ReadXref(int xrefOffset)
{
	//暂时假设xref格式比较正规，空格只有一个，免过滤空格 while (PDFInByte[++index] == 32) ;
	int index = xrefOffset;
	Xref xref=(Xref)malloc(sizeof(_Xref));
	xref->xrefOffset=xrefOffset;
	xref->newPrev=0;
	xref->newXrefOffset=0;
	xref->sections=InitList();
	//若前四个字节不是xref，特殊情况，直接返回；否则结束时index指向xref后面一个字节
	if (PDFInByte[index++] != 120 || PDFInByte[index++] != 114 || PDFInByte[index++] != 101 || PDFInByte[index++] != 102)
	{
		printf("Unknown PDF format!");
		readable = false;
		exit(0);
		return;
	}
	else
	{
		index =omitBlank(PDFInByte,index);
		//开始读取地址列表
		int startID;
		int count;
		int lineCount;
		char *newAddr;
		CharList tBytes;
		CharList tag;
		CharList value;
		char *tString;
		char *tagString;
		char *valueString;
		//读地址列表
		do
		{
			startID =getNumber(PDFInByte,&index);
			index++;                        //免过滤，index指向count第一个字节
			count = getNumber(PDFInByte,&index);

			Section sec=(Section)malloc(sizeof(_Section));
			sec->startID=startID;
			sec->count=count;
			sec->Addrs=InitList();

			index=omitBlank(PDFInByte,index);
			lineCount = count;
			while (lineCount > 0)     //读count行结束，结束时index指向数字或者trailer的t
			{
				newAddr=(char*)malloc(sizeof(char)*19);
				for (int i = 0; i < 18; i++)            //10个字节地址，1个空格，5个字节产生号，一个空格，一个字节标志位，2个字节换行，共20字节
				{
					newAddr[i] = PDFInByte[index + i];
				}
				newAddr[18]=0;
				AddNode(sec->Addrs,newAddr);
				index += 20;
				lineCount--;
			}
			AddNode(xref->sections,sec);
		} while (PDFInByte[index] != 116);          //当读到trailer的t时结束，index指向t

		//开始读取trailer，暂时认定xref必定跟随trailer且格式正规，免验证
		tBytes=InitList();
		Trailer trailer=(Trailer)malloc(sizeof(_Trailer));
		trailer->Encrypt=0;
		trailer->Info=0;
		trailer->Prev=0;
		trailer->Root=0;
		trailer->Size=0;
		trailer->XRefStm=0;
		while (PDFInByte[++index] != 47) ;      //找第一个/，trailer至少要有Root属性，结束时指向第一个/
		while (PDFInByte[index] != 62 || PDFInByte[index + 1] != 62)            //一直读取到>>的前一个字符
		{
			AddNode(tBytes,(void*)PDFInByte[index++]);
		}
		tString=CharListGetString(tBytes);
		trailer->Prototype =tString;

		index = 0;
		while (index < tBytes->length)
		{
			//tag.Clear();
			//value.Clear();
			index++;
			tag=InitList();
			while (tString[index] != 32 && tString[index] != 91)      //结束时index指向tag后的空格或[
			{
				AddNode(tag,(void *)tString[index]);
				index++;
			}
			tagString = CharListGetString(tag);
			while (tString[index] == 32) index++;            //滤过空格，找到空格后第一个有效字符

			value=InitList();
			while (index < tBytes->length && tString[index] != 47)       //读取value，结束时index指向下一个/或到达结尾
			{
				AddNode(value,(void *)tString[index++]);
			}
			valueString = CharListGetString(value);
			int end = (int)strstr(valueString," ");//若没有空格，则strstr返回0；否则返回首个空格的地址
			end = end == 0 ? value->length : end-(int)valueString;
			valueString = Substring(valueString,0, end);
			int val;
			if(IntTryParse(valueString, &val))
			{
				switch (tagString[0])      //应该仅有以下几种情况
				{
				case 'S'://Size
					trailer->Size = val;
					if (Size < val) Size = val;
					break;
				case 'I'://Info
					trailer->Info = val;
					if (Info == 0) Info = val;
					break;
				case 'R'://Root
					trailer->Root = val;
					if (Root == 0) Root = val;
					break;
				case 'P'://Prev
					trailer->Prev = val;
					break;
				case 'E'://Encrypt
					trailer->Encrypt = val;
					if (Encrypt == 0) Encrypt = val;
					break;
				case 'X'://XRefStm
					trailer->XRefStm = val;
					break;
				case 'T'://Tail
					hasTail = true;
					if (Tail == 0) Tail = val;
					break;
				}
			}else{
				trailer->ID[0] = Substring(valueString,2, 32);
				trailer->ID[1] = Substring(valueString,36, 32);
				if (ID == NULL) {
					ID[0] = trailer->ID[0];
					ID[1] = trailer->ID[1];
				}}
		}
		if (trailer->Prev != 0)
			ReadXref(trailer->Prev);
		xref->trailer = trailer;
		AddNode(xrefs,xref);
	}
}

//检验字符串num是否是代表了十六进制数
int TestValid(char *num)
{
	while(*num!=0)
	{
		if (*num < 48 || *num > 57 && *num < 65 || *num > 70 && *num < 97 || *num > 102)
			return 0;
		num++;
	}
	return 1;
}

//将int值转化为字符串
LPSTR IntToString(int num)
{
	List list=InitList();
	int sign,i;
	LPSTR output;
	if(num==0)
		return "0";
	sign=num>0?1:-1;
	num=num*sign;
	while(num!=0)
	{
		AddNode(list,(void*)(num%10+48));
		num/=10;
	}
	if(sign==-1)
	{
		AddNode(list,(void*)'-');
	}
	output=(LPSTR)malloc(sizeof(char)*(list->length+1));
	for(i=0;i<list->length;i++)
	{
		*(output+i)=(char)GetNodeData(list,list->length-1-i);
	}
	*(output+i)=0;
	return output;
}

//将代码页（Code Page）指定为ANSI代码页
#define CP CP_ACP
//将ASCII转化为Unicode
LPWSTR MByteToWChar(LPCSTR lpcStr)
{
	LPWSTR lpwStr;
	int dwSize;
	dwSize=2*MultiByteToWideChar(CP,0,lpcStr,-1,NULL,0);
	(lpwStr)=(LPWSTR)malloc(dwSize);
	MultiByteToWideChar(CP,0,lpcStr,-1,lpwStr,dwSize);
	return lpwStr;
}

//将Unicode转化为ASCII
LPSTR WCharToMByte(LPCWSTR lpcwStr)
{
	LPSTR lpStr;
	int dwSize;
	dwSize=WideCharToMultiByte(CP,0,lpcwStr,-1,NULL,0,NULL,FALSE);
	lpStr=(LPSTR)malloc(dwSize);
	WideCharToMultiByte(CP,0,lpcwStr,-1,lpStr,dwSize,"#",FALSE);
	return lpStr;
}//有个问题，诸如®这样的符号，无法转化

void printSBytes(LPSTR ptr,int len)
{
	int i;
	for(i=0;i<len;i++)
	{
		if((unsigned char)*(ptr+i)!=0)
			printf("%d ",(unsigned char)*(ptr+i));
		else
			printf("0 ");
	}
	printf("\n");
}

void printWBytes(LPWSTR ptr)
{
	int i,size=(int)wcslen(ptr)*2;
	for(i=0;i<size;i++)
	{
		printf("%d ",(unsigned char)*((LPSTR)ptr+i));
	}
	printf("\n");
}

//颠倒Unicode的字节顺序
LPWSTR UnicodeByteReverse(LPWSTR src)
{
	int i,size=(int)wcslen(src)*2;
	LPSTR out=(LPSTR)malloc(sizeof(char)*(size+2));
	for(i=0;i<size;i+=2)
	{
		*(out+i+1)=*((LPSTR)src+i);
		*(out+i)=*((LPSTR)src+i+1);
	}
	*(out+size)=0;
	*(out+size+1)=0;
	return (LPWSTR)out;
}

//将Unicode翻译为UTF8
LPSTR UnicodeToUTF8(LPCWSTR src)
{
	LPSTR out;
	int i,j,size=(int)wcslen(src),hex;
	out=(LPSTR)malloc(sizeof(char)*(2*(size*3+1)));//一个字节的'\0'就以两个字节的0代替
	for(i=0,j=0;i<size;i++)
	{
		hex=*(src+i);
		if(hex<0x7F)
		{
			*(out+j)=hex;
			j++;
		}
		else if(hex<0x7FF)
		{
			*(out+j)=hex/0x40+0xC0;
			*(out+j+1)=hex%0x40+0x80;
			j+=2;
		}
		else if(hex<0xFFFF)
		{
			*(out+j)=hex/0x1000+0xE0;
			hex%=0x1000;
			*(out+j+1)=hex/0x40+0x80;
			*(out+j+2)=hex%0x40+0x80;
			j+=3;
		}//暂时只遇到3个字节的UTF-8编码
	}
	*(out+j)=0;
	return out;
}

//将UTF8翻译为Unicode
LPWSTR UTF8ToUnicode(LPSTR src)
{
	LPWSTR out;
	int i,j,size=(int)strlen(src);
	unsigned short int hex;
	out=(LPWSTR)malloc(sizeof(char)*2*(size+1));
	for(i=0,j=0;i<size;i++)
	{
		hex=(unsigned char)*(src+i);
		if(hex<0x80)
		{
			*(out+j)=hex;
			j++;
		}
		else if(hex<0xE0)
		{
			hex-=0xC0;
			hex*=0x40;
			hex+=(unsigned char)*(src+(++i))-0x80;
			*(out+j)=hex;
			j++;
		}
		else if(hex<0xF0)
		{
			hex-=0xE0;
			hex*=0x40;
			hex+=(unsigned char)*(src+(++i))-0x80;
			hex*=0x40;
			hex+=(unsigned char)*(src+(++i))-0x80;
			*(out+j)=hex;
			j++;
		}
	}
	*(out+j)=0;
	return out;
}

//将Tag翻译为字符串，此处的Tag就是键值对中的键
LPWSTR ReadTagString(char *tagString)//pdf中#23代表'#'
{
	int index = 0;
	int len=(int)strlen(tagString);
	List output=InitList();//每个data是byte值
	while (index < len)
	{
		if (tagString[index] != '#')
		{
			AddNode(output,(void*)tagString[index] );
			index++;
		}
		else
		{
			if (tagString[index + 1] == '#')//正常pdf几乎不可能遇到的情况，两个#相连
			{
				AddNode(output,(void*)'#');
				AddNode(output,(void*)'#');
				index += 2;
			}
			else
			{
				if (!TestValid(Substring(tagString,index + 1, 2)))//如果不符合#00~#FF的形式，原样输出，正常pdf几乎不可能遇到的情况
				{
					AddNode(output,(void*)'#');
					AddNode(output,(void*)tagString[index+1]);
					AddNode(output,(void*)tagString[index+2]);
					index+=3;
				}
				else
				{
					AddNode(output,(void*)GetDecimal(Substring(tagString,index + 1, 2)));
					index += 3;
				}
			}
		}
	}
	return UTF8ToUnicode(CharListGetString(output));
}

//读取键值对中的键值
int ReadValue(int start, LPWSTR *valueString)         //start指向属性的第一个字符
{
	List value = InitList();
	int cursor = start;
	if (PDFInByte[cursor] == 254 && PDFInByte[cursor + 1] == 255)       //BigEndianUnicode的情况
	{
		cursor += 2;
		while (PDFInByte[cursor] != 41)//读到)结束 
		{
			if (PDFInByte[cursor] == 92) cursor++;             //PDF文件逢92要跳过一次
			AddNode(value,(void*)PDFInByte[cursor++]);
			if (PDFInByte[cursor] == 92) cursor++;
			AddNode(value,(void*)PDFInByte[cursor++]);
		}
		AddNode(value,0);//Unicode 补一个0，再加上CharListGetString中补的0，构成结束符
		*valueString=UnicodeByteReverse((LPWSTR)CharListGetString(value));
	}
	else//ASCII的情况
	{
		while (PDFInByte[cursor] != 41)//读到)结束 
		{
			if (PDFInByte[cursor] == 92) cursor++;      //需要转义的情况，跳一位
			AddNode(value,(void*)PDFInByte[cursor++]);
		}
		*valueString=MByteToWChar(CharListGetString(value));
	}
	return cursor;          //cursor最后指向)
}

const LPSTR InfoTags[]={"Title","Author","Subject","Keywords","CreationDate","ModDate","Creator","Producer"};

//读取PDF的信息
void ReadPath(char *PDFPath)
{
	FILE *fp;
	if((fp=fopen(PDFPath,"rb"))==0)
	{
		printf("cannot open %s\n",PDFPath);
	}
	Title=L"";
	Author=L"";
	Subject=L"";
	Keywords=L"";
	CreationDate=L"";
	ModDate=L"";
	Creator=L"";
	Producer=L"";
	xrefs =InitList();
	Tails=InitList();
	Attrs=InitList();
	FileLength=getFileSize(PDFPath);
	printf("FileLength: %d\n",FileLength);
	PDFInByte=(unsigned char*)malloc(sizeof(char)*FileLength);
	fread(PDFInByte,1,FileLength,fp);
	fclose(fp);
	Version[0]=PDFInByte[5];
	Version[1]= '.' ;
	Version[2]=PDFInByte[7];
	Version[3]=0;
	printf("Version: %s\n",Version);

	int index = FileLength;
	index = findStringUp(PDFInByte,FileLength,index, "%%EOF") - 5;
	while (PDFInByte[index] > 57 || PDFInByte[index] < 48) index--;
	int i = 1,j;
	StartXref = 0;

	while (PDFInByte[index] <= 57 && PDFInByte[index] >= 48)
	{
		StartXref += (PDFInByte[index--] - 48) * i;
		i *= 10;
	}
	printf("StartXref: %d\n",StartXref);

	ReadXref(StartXref);

	LPSTR* Addrs=(LPSTR*)malloc(sizeof(char*)*Size);
	Xref xref;
	Section section;
	for(i=0;i<xrefs->length;i++)
	{
		xref=(Xref)GetNodeData(xrefs,i);
		for(j=0;j< xref->sections->length;j++)
		{
			section=(Section)GetNodeData(xref->sections,j);
			for (int cursor = 0; cursor < section->count; cursor++)
			{
				Addrs[section->startID+cursor]=(LPSTR)GetNodeData(section->Addrs,cursor); //用新的覆盖旧的
			}
		}
	}

	//读取Info
	if (Info == 0) return;
	index=InfoAt = IntParse(Substring(Addrs[Info],0, 10));

	while (PDFInByte[index++] != 47) ; //找到第一个/,结束时index指向tag的第一个字符
	List tag = InitList();
	LPSTR tagString;
	LPWSTR valueString;
	valueString=(LPWSTR)malloc(sizeof(wchar_t));
	while (PDFInByte[index] != 62)//>
	{
		//tag.Clear();
		tag = InitList();
		while (PDFInByte[index] != 32 && PDFInByte[index] != 40) //找到空格或者(，结束时index指向空格或(
		{
			AddNode(tag,(void*)PDFInByte[index]);
			index++;
		}
		index = omitBlank(PDFInByte,index);
		tagString =CharListGetString(tag);
		//开始读取value
		if (PDFInByte[index] != 40)                     //指向其他obj的情况
		{
			int tagID = getNumber(PDFInByte, &index);
			index++;
			while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>

			char *tagAddr = Substring(Addrs[tagID],0, 10);
			int cursor = IntParse(tagAddr);

			while (PDFInByte[cursor++] != 40) ;          //找(，结束时指向属性的第一个字符
			ReadValue(cursor,&valueString);
		}
		else  //跟随的括号中有属性的情况
		{
			index = ReadValue(++index, &valueString) + 1;            //index此时指向)后面一个字符
			while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>
		}
		index++;
		//index最终定位在下一个tag的第一个字符或结束符>

		int TagNum=-1,i;
		for(i=0;i<8;i++)
		{
			if(strcmp(tagString,InfoTags[i])==0)
			{
				TagNum=i;
				break;
			}
		}
		switch (TagNum)
		{
		case 0: Title = valueString; break;//Title
		case 1: Author = valueString; break;//Author
		case 2: Subject = valueString; break;//Subject
		case 3: Keywords = valueString; break;//Keywords
		case 4: CreationDate = valueString; break;
		case 5: ModDate =valueString; break;//ModDate
		case 6: Creator = valueString; break;//Creator
		case 7: Producer = valueString; break;//Producer
		default: 
			Attr attr=(Attr)malloc(sizeof(_Attr));
			attr->Name=tagString;
			attr->Value=valueString;
			AddNode(Attrs,attr); break;
		}
	}
	while (PDFInByte[index] != 106) index++;//将index指向endobj的j
	InfoEnd = index + 1;


	//读取Tail
	if (Tail == 0) return;
	index=TailAt = IntParse(Substring(Addrs[Tail],0, 10));


	while (PDFInByte[index++] != 47) ; //找到第一个/,结束时index指向tag的第一个字符
	while (PDFInByte[index] != 62)//>
	{
		//tag.Clear();
		tag=InitList();
		while (PDFInByte[index] != 32 && PDFInByte[index] != 40) //找到空格或者(，结束时index指向空格或(
		{
			AddNode(tag,(void*)PDFInByte[index]);
			index++;
		}
		index = omitBlank(PDFInByte,index);
		tagString =CharListGetString(tag);

		//开始读取value，默认Tail的属性只以空号的形式保存
		index = ReadValue(++index, &valueString) + 1;             //index此时指向)后面一个字符
		while (PDFInByte[index] != 47 && PDFInByte[index] != 62) index++;             //找下一个/或者结束符>，结束时index指向下一个/或结束符>

		index++;
		//index最终定位在下一个tag的第一个字符或结束符>
		Attr tail=(Attr)malloc(sizeof(_Attr));
		tail->Name=tagString;
		tail->Value=valueString;
		AddNode(Tails,tail); 
	}
	while (PDFInByte[index] != 106) index++;//将index指向endobj的j
	TailEnd = index + 1;
	fclose(fp);//已经关闭过了？
}

void showInfo()
{
	//StringList show=InitStringList();
	int i;
	Xref xref;
	for(i=0;i<xrefs->length;i++){
		xref=(Xref)GetNodeData(xrefs,i);
		printf("%d----\n",i+1);
		printf("XrefOffset: %d\n",xref->xrefOffset);
		printf("Prev: %d\n",xref->trailer->Prev);
		printf("XRefStm: %d\n",xref->trailer->XRefStm);
	}
	printf( "----\n");
	printf("InfoAt: %d\n",InfoAt);
	printf("%d xrefs\n", xrefs->length);
	for(i=0;i<xrefs->length;i++){
		xref=(Xref)GetNodeData(xrefs,i);
		printf(	 "%s\n",xref->trailer->Prototype) ;
	}
	printf("PDFInfo:\n");
	printf("Title: ");
	printUnicode(Title);
	printf("\nAuthor: ");
	printUnicode(Author);
	printf("\nSubject: ");
	printUnicode(Subject);
	printf("\nKeywords: ");printUnicode(Keywords);
	printf("\nCreationDate: ");printUnicode(CreationDate);
	printf("\nModDate: ");printUnicode(ModDate);
	printf("\nCreator: ");printUnicode(Creator);
	printf("\nProducer: ");printUnicode(Producer);
	printf("\nAdditive Attributes:\n");
	Attr attr;
	for(i=0;i<Attrs->length;i++){
		attr=(Attr)GetNodeData(Attrs,i);
		printf("%d\t",i+1);
		printUnicode(ReadTagString(attr->Name));
		printf("\t");
		printUnicode(attr->Value);
		printf("\n");
	}
	if(Tail==0)return;
	printf("\nTail Attributes:\n");
	for(i=0;i<Tails->length;i++){
		attr=(Attr)GetNodeData(Tails,i);
		printf("%d\t",i+1);
		printUnicode(ReadTagString(attr->Name));
		printf("\t");
		printUnicode(attr->Value);
		printf("\n");
	}
	/*return show;*/
}

//向writer中添加s对应的ASCII码
void AddASCII(List writer, LPSTR s)
{
	int i,len=(int)strlen(s);
	for(i=0;i<len;i++){
		AddNode(writer,(void*)s[i]);
	}
}

//向writer中添加s对应的BigEndianUnicode
void AddBigEndianUnicode(List writer, LPWSTR s)
{
	if (s == NULL || s == L"") return;
	AddNode(writer,(void*)254);
	AddNode(writer,(void*)255);
	s=UnicodeByteReverse(s);
	int i,len=(int)wcslen(s)*2;
	for(i=0;i<len;i++)
	{
		if(*((LPSTR)s+i)==92)AddNode(writer,(void*)92);
		AddNode(writer,(void*)*((LPSTR)s+i));
	}
}

int SaveXrefs(FILE *fp, int index, int gap, int from, int to, int newStartxref,int boundary)
{
	List writer = InitList();
	LPSTR str = "";
	int cursor,i,len;
	Xref xref;
	for (cursor = from; cursor < to; cursor++)
	{
		//xref之前
		xref=(Xref)GetNodeData(xrefs,cursor);
		while (index < xref->xrefOffset)
		{
			fputc(PDFInByte[index++],fp);
		}
		//xref地址部分
		while (PDFInByte[index] != 13 && PDFInByte[index] != 10) fputc(PDFInByte[index++],fp);
		while (PDFInByte[index] == 13 || PDFInByte[index] == 10) fputc(PDFInByte[index++],fp);
		len=xref->sections->length;
		Section sec;
		for (i=0;i<len;i++)
		{
			int count,j;
			sec=(Section)GetNodeData(xref->sections,i);
			while (PDFInByte[index] != 13 && PDFInByte[index] != 10)  fputc(PDFInByte[index++],fp);
			while (PDFInByte[index] == 13 || PDFInByte[index] == 10)  fputc(PDFInByte[index++],fp);
			for (count = 0; count < sec->count; count++)
			{
				char Addr[11];
				for (j = 0; j < 10; j++)
				{
					Addr[j] = PDFInByte[index + j];
				}
				Addr[j]=0;
				int num=IntParse(Addr);
				if (num > boundary)
				{
					num += gap;
					for (int j = 0; j < 10; j++)
					{
						Addr[9 - j] = num % 10 + 48;
						num /= 10;
					}
				}
				fputs(Addr,fp);
				index += 10;
				for (j = 0; j < 10; j++)
				{
					fputc(PDFInByte[index++],fp);
				}
			}
		}
		//trailer的处理
		while (PDFInByte[index] != 62 || PDFInByte[index + 1] != 62)
		{
			if (PDFInByte[index] == 80 && PDFInByte[index + 1] == 114 && PDFInByte[index + 2] == 101 && PDFInByte[index + 3] == 118)//Prev
			{
				fputs("Prev ",fp);
				index +=(int)strlen("Prev ");
				str=IntToString(xref->newPrev);
				fputs(str,fp);
				index += (int)strlen(str);
				continue;
			}
			if (PDFInByte[index] == 88 && PDFInByte[index + 1] == 82 && PDFInByte[index + 2] == 101 && PDFInByte[index + 3] == 102)//XRef
			{
				writer=InitList();
				int len = (int)strlen("XRefStm ")+(int)strlen(IntToString(xref->trailer->XRefStm));
				index += len;
				fputs("DelTag ",fp);
				for (i = 0; i < len - 7; i++)fputc('0',fp);
				continue;
			}
			fputc(PDFInByte[index++],fp);
		}//结束时index指向>>的第一个>
		//看是否有startxref
		if (cursor == xrefs->length - 1)
		{
			int see = index + 2;
			while (PDFInByte[see] == 13 || PDFInByte[see] == 10) see++; //结束时指向下一行第一个字符
			//若有start
			if (PDFInByte[see] == 115 && PDFInByte[see + 1] == 116 && PDFInByte[see + 2] == 97 && PDFInByte[see + 3] == 114 && PDFInByte[see + 4] == 116)
			{
				while (PDFInByte[see] < 48 || PDFInByte[see] > 57) see++;//see定位在第一个数字
				while (index < see) fputc(PDFInByte[index++],fp);//结束时index指向第一个数字

				str = IntToString(newStartxref);
				fputs(str,fp);
				index += (int)strlen(str);

				if (cursor == xrefs->length- 1)
				{
					while (index < FileLength)
					{
						fputc(PDFInByte[index++],fp);
					}
					break;
				}
			}
		}
	}
	return index;
}

//将tagName字符串翻译为UTF8的字节流，即生成键的字节流
LPSTR ProduceNameString(LPWSTR tagName)
{
	int dec,len=(int)wcslen(tagName),i,j,k;
	List value=InitList();
	LPSTR utf8,hex=(LPSTR)malloc(sizeof(char)*2);
	LPWSTR charString=(LPWSTR)malloc(sizeof(wchar_t)*2);
	*(charString+1)=0;
	for(i=0;i<len;i++)
	{
		if (tagName[i] < 48 || tagName[i] > 57 && tagName[i] < 65 || tagName[i] > 90 && tagName[i] < 97 || tagName[i] > 122)
		{
			*charString=tagName[i];
			utf8=UnicodeToUTF8(charString);
			int templen=(int)strlen(utf8);
			for(j=0;j<templen;j++)
			{
				dec = (unsigned char)*((LPSTR)utf8+j);
				hex[0]=hex[1]=48;
				k=1;
				while (dec != 0)
				{
					switch (dec % 16)
					{
					case 0:hex[k]='0'; break;
					case 1: hex[k]='1'; break;
					case 2: hex[k]='2'; break;
					case 3: hex[k]='3'; break;
					case 4: hex[k]='4'; break;
					case 5: hex[k]='5'; break;
					case 6: hex[k]='6'; break;
					case 7: hex[k]='7'; break;
					case 8: hex[k]='8'; break;
					case 9: hex[k]='9'; break;
					case 10: hex[k]='A'; break;
					case 11: hex[k]='B'; break;
					case 12: hex[k]='C'; break;
					case 13: hex[k]='D'; break;
					case 14: hex[k]='E'; break;
					case 15: hex[k]='F'; break;
					}
					dec /= 16;
					k--;
				}
				AddNode(value,(void*)'#');
				for(k=0;k<2;k++)
				{
					AddNode(value,(void*)hex[k]);
				}
			}
		}
		else
		{
			AddNode(value,(void*)tagName[i]);
		}
	}
	return CharListGetString(value);
}

//保存Info和自定义属性
void SaveInfo(LPWSTR title, LPWSTR author, LPWSTR subject, LPWSTR keywords, List attrs)
{
	FILE *fp;
	if((fp=fopen(PDFPath,"wb"))==0)
	{
		printf("cannot open %s\n",PDFPath);
		system("pause");
		exit(0);
	}
	List writer = InitList();
	LPSTR str;
	//生成新的Info
	AddASCII(writer, IntToString(Info));
	AddASCII(writer, " 0 obj\r\n<</Title(");
	AddBigEndianUnicode(writer, title);
	AddASCII(writer, ")/Author(");
	AddBigEndianUnicode(writer, author);
	AddASCII(writer, ")/Subject(");
	AddBigEndianUnicode(writer, subject);
	AddASCII(writer, ")/Keywords(");
	AddBigEndianUnicode(writer, keywords);
	AddASCII(writer, ")/CreationDate(");
	AddASCII(writer, WCharToMByte(CreationDate));
	AddASCII(writer, ")/ModDate(");
	AddASCII(writer, WCharToMByte(ModDate));
	AddASCII(writer, ")/Creator(");
	AddBigEndianUnicode(writer, Creator);
	AddASCII(writer, ")/Producer(");
	AddBigEndianUnicode(writer, Producer);
	int i;
	Attr attr;
	for(i=0;i<attrs->length;i++)
	{
		attr=(Attr)GetNodeData(attrs,i);
		AddASCII(writer, ")/");
		AddASCII(writer, attr->Name);
		AddASCII(writer, "(");
		AddBigEndianUnicode(writer, attr->Value);
	}
	AddASCII(writer, ")>>\r\nendobj");


	int index = 0;
	int pointA = InfoAt;
	int pointB = InfoEnd;
	int gap = writer->length - pointB + pointA;
	//更新xrefs
	Xref xref;
	for (i = 0; i < xrefs->length; i++)
	{
		xref=(Xref)GetNodeData(xrefs,i);
		xref->newXrefOffset = xref->xrefOffset;
		xref->newPrev = xref->trailer->Prev;
		if (i > 0)
		{
			xref->newPrev = ((Xref)GetNodeData(xrefs,i-1))->newXrefOffset;
		}
		if (xref->xrefOffset > pointA)        //未检测位数变化
		{
			xref->newXrefOffset += gap;
		}
		if (xref->trailer->XRefStm > pointA)
		{
			xref->trailer->XRefStm += gap;
		}
	}
	int newStartxref = xref->newXrefOffset;
	List newXrefs=InitList();
	int j,min,offset=0;
	Xref minOffsetXref;
	for(i=0;i<xrefs->length;i++)//鉴于xref不会很多，暂时用n^2的低效算法
	{
		min=MAXINT;
		for(j=0;j<xrefs->length;j++)
		{
			xref=(Xref)GetNodeData(xrefs,j);
			if(xref->xrefOffset<=offset)continue;
			if(xref->xrefOffset<min){
				min=xref->xrefOffset;
				minOffsetXref=xref;
			}
		}
		offset=minOffsetXref->xrefOffset;
		AddNode(newXrefs,minOffsetXref);
	}
	xrefs=newXrefs;
	int pos = 0;
	for(i=0;i<xrefs->length;i++)   //pos确定Info在Xrefs的排位,指示在哪一个xref之前
	{
		if (((Xref)GetNodeData(xrefs,i))->xrefOffset < pointA) pos++;
		else break;
	}

	index=SaveXrefs(fp, index, gap, 0, pos, newStartxref,InfoAt);
	//写到Info之前,此处不能fwrite因为起始点不为0
	while (index < pointA)
	{
		fputc(PDFInByte[index++],fp);
	}
	//重写Info
	fwrite(CharListGetString(writer),sizeof(char),writer->length,fp);
	//写Info之后
	index = pointB;
	index=SaveXrefs(fp,index, gap, pos, xrefs->length, newStartxref,InfoAt);
	fclose(fp);
}

void AppendTail(List tails)
{
	FILE *fp;
	int i;
	List writer = InitList();

	Tail = hasTail ? Tail : Size;
	AddASCII(writer,IntToString(Tail));
	AddASCII(writer," 0 obj\r\n<<");
	Attr tail;
	for (i=0;i<tails->length;i++)
	{
		tail=(Attr)GetNodeData(tails,i);
		AddASCII(writer,"/");
		AddASCII(writer,tail->Name);
		AddASCII(writer, "(");
		AddBigEndianUnicode(writer, tail->Value);
		AddASCII(writer,")");
	}
	AddASCII(writer, ">>\r\nendobj");
	if (hasTail)
	{
		if((fp=fopen(PDFPath,"wb"))==0)
		{
			printf("cannot open %s\n",PDFPath);
			system("pause");
			exit(0);
		}

		int gap = writer->length - TailEnd + TailAt;
		//更新xrefs
		Xref xref;
		for (i = 0; i < xrefs->length; i++)
		{
			xref=(Xref)GetNodeData(xrefs,i);
			xref->newXrefOffset = xref->xrefOffset;
			xref->newPrev = xref->trailer->Prev;
			if (i > 0)
			{
				xref->newPrev = ((Xref)GetNodeData(xrefs,i-1))->newXrefOffset;
			}
			if (xref->xrefOffset > TailAt)        //未检测位数变化
			{
				xref->newXrefOffset += gap;
			}
			if (xref->trailer->XRefStm > TailAt)
			{
				xref->trailer->XRefStm += gap;
			}
		}
		int newStartxref = xref->newXrefOffset;
		List newXrefs=InitList();
		int j,min,offset=0;
		Xref minOffsetXref;
		for(i=0;i<xrefs->length;i++)//鉴于xref不会很多，暂时用n^2的低效算法
		{
			min=MAXINT;
			for(j=0;j<xrefs->length;j++)
			{
				xref=(Xref)GetNodeData(xrefs,j);
				if(xref->xrefOffset<=offset)continue;
				if(xref->xrefOffset<min){
					min=xref->xrefOffset;
					minOffsetXref=xref;
				}
			}
			offset=minOffsetXref->xrefOffset;
			AddNode(newXrefs,minOffsetXref);
		}
		xrefs=newXrefs;
		int pos = 0;
		for(i=0;i<xrefs->length;i++)   //pos确定Info在Xrefs的排位,指示在哪一个xref之前
		{
			if (((Xref)GetNodeData(xrefs,i))->xrefOffset < TailAt) pos++;
			else break;
		}

		//重写到TailAt，思考能不能直接setlength！！！
		fwrite(PDFInByte,sizeof(char),TailAt,fp);
		//重写Tail
		fwrite(CharListGetString(writer),sizeof(char),writer->length,fp);		
		//写Tail之后，xref之前
		SaveXrefs(fp,TailEnd, gap, pos, xrefs->length, newStartxref,TailAt);

	}
	else
	{
		if((fp=fopen(PDFPath,"ab"))==0)
		{
			printf("cannot open %s\n",PDFPath);
			system("pause");
			exit(0);
		}
		//写Tail前的\r\n
		fputs("\r\n",fp);
		fwrite(CharListGetString(writer),sizeof(char),writer->length,fp);//此处不能fputs！因为有特殊编码方式会被fputs认为是结束符，提前结束
		TailAt = FileLength + 2;//+2因为Tail对象前面补充了\r\n
		TailEnd = TailAt + writer->length;
		Tail = Size;
		fputs("\r\nxref\r\n",fp);
		fputs(IntToString( Tail),fp);
		fputs(" 1\r\n",fp);
		for (i = 0; i < 10 - strlen(IntToString(TailAt)); i++){
			fputs("0",fp);
		}
		fputs(IntToString(TailAt),fp);
		fputs(" 00000 n\r\ntrailer\r\n<</Size ",fp);
		fputs(IntToString(++Size),fp);
		fputs("/Root ",fp);
		fputs(IntToString(Root),fp);
		fputs(" 0 R/Info ",fp);
		fputs(IntToString(Info),fp);				
		fputs( " 0 R/Prev ",fp);
		fputs(IntToString(StartXref),fp);
		fputs( "/Tail ",fp);
		fputs(IntToString(Tail),fp);
		fputs(" 0 R>>\r\nstartxref\r\n",fp);
		fputs(IntToString(TailEnd+2),fp);
		fputs( "\r\n%%EOF",fp);//writer此前保存的是Tail的信息，没有首尾的\r\n

	}
	fclose(fp);
}

int _tmain(int argc, _TCHAR* argv[])
{
	//pdf原版(是0 92 40，但用0 40也可以？？
	PDFPath="Z:/My Folder/Files/PDF相关/PDFs/Yes/老友记-六人行-Friends-老友妙语录.pdf";
	ReadPath(PDFPath);
	showInfo();
	List attrs=InitList();
	Attr attr=(Attr)malloc(sizeof(_Attr));
	attr->Name=ProduceNameString(L"学校®");
	attr->Value=L"北航®";
	AddNode(attrs,attr);
	attr=(Attr)malloc(sizeof(_Attr));
	attr->Name=ProduceNameString(L"学院®");
	attr->Value=L"计算机®";
	AddNode(attrs,attr);
	SaveInfo(L"标题®",L"作者®",L"主题®",L"关键词®",attrs);
	List tails=InitList();
	Attr tail=(Attr)malloc(sizeof(_Attr));
	tail->Name=ProduceNameString(L"啊啊啊啊®");
	tail->Value=L"1112®";
	AddNode(tails,tail);
	tail=(Attr)malloc(sizeof(_Attr));
	tail->Name=ProduceNameString(L"ooo®");
	tail->Value=L"计算机®";
	AddNode(tails,tail);
	ReadPath(PDFPath);
	AppendTail(tails);
	ReadPath(PDFPath);
	showInfo();
	system("pause");
	return 0;
}

