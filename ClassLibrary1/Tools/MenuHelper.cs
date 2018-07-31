namespace DAL.Tools
{
    public class MenuHelper
    {
        public static string GetMainMenu(int roleId)
        {
            switch (roleId)
            {
                // 公司领导
                case 1:
                    return @"<li id='itemC'><a href='/Companys?mmenu=Companys'><i class='parent-nav-icon icon-waitread'></i><span class='span2'  id='spanlaowu'>劳务承分包企业</span></a></li>
                            <li id='itemP'><a href='/Projects?mmenu=Projects'><i class='parent-nav-icon icon-meetroom'></i><span>招标内容</span></a></li>
                            <li id='itemF'><a href='/BidingFile?mmenu=BidingFile'><i class='parent-nav-icon icon-carmanage'></i><span>招标文件</span></a></li>
                            <li id='itemB'><a href='/Bidings?mmenu=Bidings'><i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id='itemM'><a href='/MakeBidFile?mmenu=MakeBidFile'><i class='parent-nav-icon icon-cardprint'></i><span>定标管理</span></a></li>
                            <li id='itemS'><a href='/Statistic?mmenu=Statistic'><i class='parent-nav-icon icon-sealapply'></i><span>统计分析</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?mmenu=SystemSetup'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                case 2:
                    // 管理员
                    return @"<li id='itemC'><a href='/Companys?mmenu=Companys'><i class='parent-nav-icon icon-waitread'></i><span class='span2'  id='spanlaowu'>劳务承分包企业</span></a></li>
                            <li id='itemP'><a href='/Projects?mmenu=Projects'><i class='parent-nav-icon icon-meetroom'></i><span>招标内容</span></a></li>
                            <li id='itemF'><a href='/BidingFile?mmenu=BidingFile'><i class='parent-nav-icon icon-carmanage'></i><span>招标文件</span></a></li>
                            <li id='itemB'><a href='/Bidings?mmenu=Bidings'><i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id='itemM'><a href='/MakeBidFile?mmenu=MakeBidFile'><i class='parent-nav-icon icon-cardprint'></i><span>定标管理</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?mmenu=SystemSetup'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                case 3:
                    // 审批部门
                    return @"<li id='itemC'><a href='/Companys?mmenu=Companys'><i class='parent-nav-icon icon-waitread'></i><span class='span2'  id='spanlaowu'>劳务承分包企业</span></a></li>
                            <li id='itemP'><a href='/Projects?mmenu=Projects'><i class='parent-nav-icon icon-meetroom'></i><span>招标内容</span></a></li>
                            <li id='itemF'><a href='/BidingFile?mmenu=BidingFile'><i class='parent-nav-icon icon-carmanage'></i><span>招标文件</span></a></li>
                            <li id='itemB'><a href='/Bidings?mmenu=Bidings'><i class='parent-nav-icon icon-meetroom'></i><span>招标管理</span></a></li>
                            <li id='itemM'><a href='/MakeBidFile?mmenu=MakeBidFile'><i class='parent-nav-icon icon-cardprint'></i><span>定标管理</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?mmenu=SystemSetup'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                case 4:
                    // 项目部
                    return @"<li id='itemC'><a href='/Companys?mmenu=Companys'><i class='parent-nav-icon icon-waitread'></i><span class='span2'  id='spanlaowu'  id='spanlaowu'>劳务承分包企业</span></a></li>
                            <li id='itemP'><a href='/Projects?mmenu=Projects'><i class='parent-nav-icon icon-meetroom'></i><span>招标内容</span></a></li>
                            <li id='itemSS'><a href='/SystemSetup?mmenu=SystemSetup'><i class='parent-nav-icon icon-sysset'></i><span>系统设置</span></a></li>";
                default:
                    return "";
            }
        }

        public static string GetSecondMenu(string mmenu, int roleId)
        {
            switch (roleId)
            {
                // 公司领导
                case 1:
                    switch (mmenu) {
                        case "Companys":
                            return @"<li><a href='/Companys' class='active'><i class='icon icon-eye'></i>名录内单位</a></li>
                                    <li><a href='/Companys/CompanysOut'><i class='icon icon-hand'></i>名录外单位</a></li>
                                    <li><a href='/Companys/MyAudit'><i class='icon icon-edit'></i>我审核的</a></li>
                                    <li><a href='/Companys/MyRecommend'><i class='icon icon-user'></i>我提交的</a></li>";
                        case "Projects":
                            return "<li><a href='/Projects' class='active'><i class='icon icon-eye1'></i>内容列表</a></li>";
                        case "BidingFile":
                            return @"<li> <a href='/BidingFile' class='active'> <i class='icon icon-eye1'> </i> 招标文件</a> </li>
                                    <li> <a href='/BidingFile/FileApprove'> <i class='icon icon-hand3'> </i> 文件审批</a> </li>";
                        case "Bidings":
                            return @"<li><a href='/Bidings' class='active'><i class='icon icon-eye1'></i>招标信息</a></li>
                                    <li><a href='/BidingApprove'><i class='icon icon-user1'></i>招标审批</a></li>";
                        case "MakeBidFile":
                            return @"<li><a href='/MakeBidFile' class='active'><i class='icon icon-eye1'></i>定标文件</a></li>
                                    <li><a href='/MakeBidFile/FileApprove'><i class='icon icon-user1'></i>文件审批</a></li>";
                        case "Statistic":
                            return "<li><a href='#' class='active'><i class='icon icon-eye1'></i>统计分析</a></li>";
                        case "SystemSetup":
                            return @"<li><a href='/SystemSetup/PersonalCenter'><i class='icon icon-hand2'></i>个人中心</a></li>";
                    }
                    return "";
                case 2:
                    // 管理员
                    switch (mmenu)
                    {
                        case "Companys":
                            return @"<li><a href='/Companys' class='active'><i class='icon icon-eye'></i>名录内单位</a></li>
                                    <li><a href='/Companys/CompanysOut'><i class='icon icon-hand'></i>名录外单位</a></li>
                                    <li><a href='/Companys/MyAudit'><i class='icon icon-edit'></i>我审核的</a></li>
                                    <li><a href='/Companys/MyRecommend'><i class='icon icon-user'></i>我提交的</a></li>";
                        case "Projects":
                            return "<li><a href='/Projects' class='active'><i class='icon icon-eye1'></i>内容列表</a></li>";
                        case "BidingFile":
                            return @"<li> <a href='/BidingFile' class='active'> <i class='icon icon-eye1'> </i> 招标文件</a> </li>
                                    <li> <a href='/BidingFile/FileApprove'> <i class='icon icon-hand3'> </i> 文件审批</a> </li>";
                        case "Bidings":
                            return @"<li><a href='/Bidings' class='active'><i class='icon icon-eye1'></i>招标信息</a></li>
                                    <li><a href='/BidingApprove'><i class='icon icon-user1'></i>招标审批</a></li>";
                        case "MakeBidFile":
                            return @"<li><a href='/MakeBidFile' class='active'><i class='icon icon-eye1'></i>定标文件</a></li>
                                    <li><a href='/MakeBidFile/FileApprove'><i class='icon icon-user1'></i>文件审批</a></li>";
                        case "Statistic":
                            return "<li><a href='#' class='active'><i class='icon icon-eye1'></i>统计分析</a></li>";
                        case "SystemSetup":
                            return @"<li><a href='/SystemSetup' class='active'><i class='icon icon-eye1'></i>组织结构</a></li>
                                    <li><a href='/SystemSetup/PersonalCenter'><i class='icon icon-hand2'></i>个人中心</a></li>
                                    <li><a href='/SystemSetup/SystemLog'><i class='icon icon-hand3'></i>系统日志</a></li>
                                    <li><a href='/SystemSetup/CategoryManagement'><i class='icon icon-hand4'></i>类别管理</a></li>
                                    <li><a href='/SystemSetup/SystemUser'><i class='icon icon-hand5'></i>系统用户</a></li>
                                    <li><a href='/SystemSetup/ApproveProcessManagement'><i class='icon icon-hand4'></i>审核流程</a></li>";
                    }
                    return "";
                case 3:
                    // 审批部门
                    switch (mmenu)
                    {
                        case "Companys":
                            return @"<li><a href='/Companys' class='active'><i class='icon icon-eye'></i>名录内单位</a></li>
                                    <li><a href='/Companys/CompanysOut'><i class='icon icon-hand'></i>名录外单位</a></li>
                                    <li><a href='/Companys/MyAudit'><i class='icon icon-edit'></i>我审核的</a></li>
                                    <li><a href='/Companys/MyRecommend'><i class='icon icon-user'></i>我提交的</a></li>";
                        case "Projects":
                            return "<li><a href='/Projects' class='active'><i class='icon icon-eye1'></i>内容列表</a></li>";
                        case "BidingFile":
                            return @"<li> <a href='/BidingFile' class='active'> <i class='icon icon-eye1'> </i> 招标文件</a> </li>
                                    <li> <a href='/BidingFile/FileApprove'> <i class='icon icon-hand3'> </i> 文件审批</a> </li>";
                        case "Bidings":
                            return @"<li><a href='/Bidings' class='active'><i class='icon icon-eye1'></i>招标信息</a></li>
                                    <li><a href='/BidingApprove'><i class='icon icon-user1'></i>招标审批</a></li>";
                        case "MakeBidFile":
                            return @"<li><a href='/MakeBidFile' class='active'><i class='icon icon-eye1'></i>定标文件</a></li>
                                    <li><a href='/MakeBidFile/FileApprove'><i class='icon icon-user1'></i>文件审批</a></li>";
                        case "Statistic":
                            return "<li><a href='#' class='active'><i class='icon icon-eye1'></i>统计分析</a></li>";
                        case "SystemSetup":
                            return @"<li><a href='/SystemSetup/PersonalCenter'><i class='icon icon-hand2'></i>个人中心</a></li>";
                    }
                    return "";
                case 4:
                    // 项目部
                    switch (mmenu)
                    {
                        case "Companys":
                            return @"<li><a href='/Companys' class='active'><i class='icon icon-eye'></i>名录内单位</a></li>
                                    <li><a href='/Companys/CompanysOut'><i class='icon icon-hand'></i>名录外单位</a></li>
                                    <li><a href='/Companys/MyAudit'><i class='icon icon-edit'></i>我审核的</a></li>
                                    <li><a href='/Companys/MyRecommend'><i class='icon icon-user'></i>我提交的</a></li>";
                        case "Projects":
                            return "<li><a href='/Projects' class='active'><i class='icon icon-eye1'></i>内容列表</a></li>";
                        case "BidingFile":
                            return @"<li> <a href='/BidingFile' class='active'> <i class='icon icon-eye1'> </i> 招标文件</a> </li>
                                    <li> <a href='/BidingFile/FileApprove'> <i class='icon icon-hand3'> </i> 文件审批</a> </li>";
                        case "Bidings":
                            return @"<li><a href='/Bidings' class='active'><i class='icon icon-eye1'></i>招标信息</a></li>
                                    <li><a href='/BidingApprove'><i class='icon icon-user1'></i>招标审批</a></li>";
                        case "MakeBidFile":
                            return @"<li><a href='/MakeBidFile' class='active'><i class='icon icon-eye1'></i>定标文件</a></li>
                                    <li><a href='/MakeBidFile/FileApprove'><i class='icon icon-user1'></i>文件审批</a></li>";
                        case "Statistic":
                            return "<li><a href='#' class='active'><i class='icon icon-eye1'></i>统计分析</a></li>";
                        case "SystemSetup":
                            return @"<li><a href='/SystemSetup/PersonalCenter'><i class='icon icon-hand2'></i>个人中心</a></li>";
                    }
                    return "";
                default:
                    return "";
            }
        }
    }
}