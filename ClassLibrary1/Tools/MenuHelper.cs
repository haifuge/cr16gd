namespace DAL.Tools
{
    public class MenuHelper
    {
        public static string GetMainMenu(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    // 管理员
                    return @"<li id='itemC'><a href='/Companys?activeMenu=itemC'><i class='parent-nav-icon icon-waitread'></i><span class='span2'>劳务成分包企业</span></a></li>
                            <li id='itemP'><a href='/Project?activeMenu=itemP'><i class='parent-nav-icon icon-meetroom'></i><span>项目管理</span></a></li>
                            <li id='itemB'><a href='/Bidings?activeMenu=itemB'><i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id='itemF'><a href='/BidingFile?activeMenu=itemF'><i class='parent-nav-icon icon-carmanage'></i><span>招标文件</span></a></li>
                            <li id='itemM'><a href='/MakeBidFile?activeMenu=itemM'><i class='parent-nav-icon icon-cardprint'></i><span>定标管理</span></a></li>
                            <li id='itemS'><a href='/Statistic?activeMenu=itemS'><i class='parent-nav-icon icon-sealapply'></i><span>统计分析</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?activeMenu=itemSS'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                case 2:
                    // 审批部门
                    return @"<li id='itemC'><a href='/Companys?activeMenu=itemC'><i class='parent-nav-icon icon-waitread'></i><span class='span2'>劳务成分包企业</span></a></li>
                            <li id = 'itemP'><a href = '/Project?activeMenu=itemP' >< i class='parent-nav-icon icon-meetroom'></i><span>项目管理</span></a></li>
                            <li id = 'itemB'><a href = '/Bidings?activeMenu=itemB' >< i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id = 'itemF'><a href='/BidingFile?activeMenu=itemF'><i class='parent-nav-icon icon-carmanage'></i><span>招标文件</span></a></li>
                            <li id = 'itemM'><a href='/MakeBidFile?activeMenu=itemM'><i class='parent-nav-icon icon-cardprint'></i><span>定标管理</span></a></li>
                            <li id = 'itemSS'><a href='/SystemSetup?activeMenu=itemSS'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                case 3:
                    // 项目部
                    return @"<li id='itemC'><a href='/Companys?activeMenu=itemC'><i class='parent-nav-icon icon-waitread'></i><span class='span2'>劳务成分包企业</span></a></li>
                            <li id='itemB'><a href='/Bidings?activeMenu=itemB'><i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?activeMenu=itemSS'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                default:
                    return "";
            }
        }
    }
}