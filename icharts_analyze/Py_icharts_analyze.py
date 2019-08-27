"""
aEmitentIds = [
aEmitentNames =
aEmitentCodes =
aEmitentMarkets
aEmitentDecp = 
aDataFormatStrs
               
aEmitentChild =
aEmitentUrls = 
"""

from urllib.request import urlopen
import time

import io

ichartsDir = ""   #"d:/SyncDirs/main/admin/sources/PyFinamDownloader/definition/"
f = io.open(ichartsDir + "icharts.js", mode='r', encoding="utf-8")


str_mas = f.readlines()
f.close()

# усекать: обрезаем строку string от того места где заканчивается подстрока strBgn и до того места где начинается подстрока strEnd
def truncate(string, strBgn, strEnd):
    return string[string.find(strBgn) + len(strBgn): string.find(strEnd)]

# усекаем строку, и затем делим по подстроке strSplt
def splitter(string, strBgn, strEnd, strSplt):
    return truncate(string, strBgn, strEnd).split(strSplt)

str_id = str_mas[0]
str_code = str_mas[2]
str_mrkt = str_mas[3]

spl = ['[', ']', ',', "[\'", "\']", "\',\'", '{', '}', "\",\""]

ids     = splitter(str_mas[0], spl[0], spl[1], spl[2]) # len = 14244
Names   = splitter(str_mas[1], spl[3], spl[4], spl[5]) # len = 14244
codes   = splitter(str_mas[2], spl[3], spl[4], spl[5]) # len = 14244
Markets = splitter(str_mas[3], spl[0], spl[1], spl[2]) # len = 14244
_Decp   = splitter(str_mas[4], spl[6], spl[7], spl[2]) # len = 14244
DtFrmSt = splitter(str_mas[5], spl[3], spl[4], spl[5]) # len = 1
Child   = splitter(str_mas[7], spl[0], spl[1], spl[2]) # len = 14244
_Urls   = splitter(str_mas[8], spl[6], spl[7], spl[8]) # len = 14244

# print(len(ids    ))
# print(len(Names  ))
# print(len(codes  ))
# print(len(Markets))
# print(len(_Decp  ))
# print(len(DtFrmSt))
# print(len(Child  ))
# print(len(_Urls  ))

lenN = len(Names)
i = 0
for i in range(1, lenN):
    print(Names[i])

    i += 1
    if i ==10:
        break


for name in Names:
    if name.find("BR") >= 0 : # name[:2] == "BR":
        print(name)

    if name == "BREN":
        print(name)



exit(0)

ch = ["\": \"", '/', ':']
Urls = []
for buf in _Urls:
    Urls.append(truncate(buf, ch[0], ch[1]))

Decp = []
tmp = []
for i, buf in enumerate(_Decp):
    Decp.append(int(buf[buf.find(ch[2]) + len(ch[2]) :]))
    if Decp[i] not in tmp:
        tmp.append(Decp[i])

s1 = 'http://export.finam.ru/'
s2 = '_790101_161231.csv?market=1&em='
s3 = '&code='
s4 = '&apply=0&df=1&mf=0&yf=1979&from=01.01.1979&dt=31&mt=11&yt=2016&to=31.12.2016&p=8&f='
s5 = '_790101_161231&e=.csv&cn='
s6 = '&dtf=1&tmf=1&MSOR=0&mstime=on&mstimever=1&sep=1&sep2=1&datf=5&at=1'

empty = [65, 85, 98, 104, 106, 134, 213, 241, 296]
fail = [61, 94, 118, 162, 200, 248, 281, 282, 284, 303]
j = 0
for i, url in enumerate(Urls):
    if url == 'moex-akcii' and Markets[i] == '1':
        j += 1
        if j < 281:
            continue
        if codes[i] == 'GAZT':
            print(j)
            break
        else:
            continue
        if j in empty or j in fail:
            print(Names[i])
            continue

        print(codes[i], ids[i])
        curFilename = codes[i]+'_790101_161231_d.csv'
        print((time.asctime())[11:19], 'try^', j, curFilename)
        curUrl = s1+codes[i]+s2+ids[i]+s3+codes[i]+s4+codes[i]+s5+codes[i]+s6
        file = urlopen(curUrl).read()
        f = open(curFilename, "wb")
        f.write(file)
        f.close()

