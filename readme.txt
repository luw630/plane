项目需要 vs2015 update3 来开

.user 文件, 每台机器不同，不便提交到 git
故拉下来之后，首次运行，Server 需要设置
"项目属性"
    Debug --
	Start external program --
		浏览到 bin_Win64 下面的 PhotonSocketServer.exe
	Start Options --
		Command line arguments --
			填  /run Default
	Workinng directory --
		浏览到 bin_Win64

注意：bin_Win64 已经包含在 git 拉下来的目录中


目录介绍：


bin_Win64 是光子 loader 所在目录
lib 是各种依赖 dll


Template_XXXXXXXXXXXXXX 这组目录用于设计 & 生成通信协议 及 配置文件

Template_Pkg      通信协议模板项目
TemplateGen_Pkg   通信协议生成器 -- VS 中右键 Debug -- Start new instance 运行, 即可在 Shared -- GenFiles 中生成最新文件.

Template_Cfg      配置文件模板项目
TemplateGen_Cfg   配置文件生成器 -- VS 中右键 Debug -- Start new instance 运行, 即可在 ............ 中找到一堆 excel ( 待续 )



新建 Server 类项目 要引用 ExitGamesLibs, Photon.SocketServer, PhotonHostRuntimeInterfaces
新建 windows Client 类项目 要引用 Photon3DotNet
u3d 要引用 Photon3Unity3D

PhotonHostRuntimeInterfaces 引用要修改属性 Embed Interop Types 为 False



读 Excel 的依赖库:

http://download.microsoft.com/download/2/4/3/24375141-E08D-4803-AB0E-10F2E3A07AAA/AccessDatabaseEngine_x64.exe