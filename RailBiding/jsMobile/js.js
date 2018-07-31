//配置选项
var host = "http://192.168.1.33:8088/";

//左侧菜单
function leftnav_htm(index) {
    var htm = '<div class="left-nav-con">' +
        '<h1>会议室</h1>' +
        '<ul class="left-nav-list">' +
        '<li><a href="index.html" class="' + (index == 1 ? "active" : "") + '"><i class="icon icon-eye"></i>查看申请</a></li>' +
        '<li><a href="list-myapply.html" class="' + (index == 2 ? "active" : "") + '"><i class="icon icon-hand"></i>我申请的</a></li>' +
        '<li><a href="list-myattend.html" class="' + (index == 3 ? "active" : "") + '"><i class="icon icon-user"></i>我参与的</a></li>' +
        '<li><a href="list-waitverify.html" class="' + (index == 4 ? "active" : "") + '"><i class="icon icon-edit"></i>待审核的</a></li>' +
        '<li><a href="list-waitread.html" class="' + (index == 5 ? "active" : "") + '"><i class="icon icon-edit"></i>待阅读的</a></li>' +
        '<li><a href="maintain-meetroom.html" class="' + (index == 6 ? "active" : "") + '"><i class="icon icon-set"></i>维护管理</a></li>' +
        '</ul>' +
        '</div>';
    $(".left-nav").append(htm);
}
//首页页面跳转tab
$(function () {
    var page_url;
    //初始化URL参数
    InitUrlParms = function () {
        var args = new Object();
        var query = location.search.substring(1);//获取查询串
        var pairs = query.split("?");//在逗号处断开
        for (var i = 0; i < pairs.length; i++) {
            var pos = pairs[i].indexOf('=');//查找name=value
            if (pos == -1) continue;//如果没有找到就跳过
            var argname = pairs[i].substring(0, pos);//提取name
            var value = pairs[i].substring(pos + 1);//提取value
            args[argname] = decodeURIComponent(value);//存为属性
        }
        page_url = args;
    };//封装好的函数，获取从a.html传来的参数type

    InitUrlParms();

    // //tab切换

    $('.tbq.content .buttons-tab a').click(function () {
        var index = $(this).index();
        $(this).addClass('active').siblings().removeClass('active');
        $('.tbk.content-block .tabs div').eq(index).addClass('active').siblings().removeClass('active');
    });
    //底部链接跳转到指定tab块

    if (page_url["type"] != null) {
        var tabIndex = page_url["type"];
    } else {
        var tabIndex = 0;
    }
    //显示指定的tab内容
    // $('.news_content>div').eq(tabIndex).show().siblings().hide();
    $('.tbk.content-block .tabs>div').eq(tabIndex).addClass('active').siblings().removeClass('active');

    //tab选项卡高亮
    $('.tbq.content .buttons-tab a').eq(tabIndex).addClass('active').siblings().removeClass('active');
});


//分页
function pages(cur, totalPage, totalRows, setNum) {
    if (!totalPage) return false;
    var url = window.location.href;
    var url_arr = url.split("?");
    var url = url_arr[0];
    var htm = '共有' + totalRows + '条，每页显示', has_para = '', set_num_htm = '<select class="page-sel">';
    $(".page").html("");
    if (url.indexOf("?") == -1) {
        has_para = "?";
    } else {
        has_para = "&";
    }
    setNum = setNum ? setNum : 5;
    for (var k = 1; k < 5; k++) {
        set_num_htm += '<option value="' + k * 5 + '" ' + (setNum == k * 5 ? "selected" : "") + '>' + k * 5 + " 条/页" + '</option>'
    }
    set_num_htm = set_num_htm + '</select>条';
    htm = htm + set_num_htm;
    htm += '<a data-cur="1" class="js-page-num page-num" style="margin-left:5px;">《</a>' +
        '<a class="js-page-num page-num num-left" data-cur="' + (cur > 1 ? (cur - 1) : 1) + '">&lt;</a>';
    if (totalPage != 1) {
        htm += '<a data-cur="' + ((cur + 1) > totalPage ? (cur - 1) : cur) + '" class="js-page-num page-num ' + (((cur + 1) > totalPage ? (cur - 1) : cur) == cur ? "active" : "") + '">' + ((cur + 1) > totalPage ? (cur - 1) : cur) + '</a>';
    }

    htm += '<a data-cur="' + ((cur + 1) > totalPage ? totalPage : (cur + 1)) + '" class="js-page-num page-num ' + (((cur + 1) > totalPage ? totalPage : (cur + 1)) == cur ? "active" : "") + '">' + ((cur + 1) > totalPage ? totalPage : (cur + 1)) + '</a>' +
        '<a class="js-page-num num-right page-num" data-cur="' + (cur < totalPage ? (cur + 1) : totalPage) + '">&gt;</a>' +
        '<a data-cur="' + totalPage + '" class="js-page-num page-num">》</a>' +
        '<a class="page-num page-input"><input type="text" value=""/></a>' +
        '<a class="js-page-num page-num num-go active" data-cur="' + ((cur + 2) > totalPage ? cur : (cur + 2)) + '">GO</a>';
    $(".page").append(htm);

    //分页点击
    $(".page>a.js-page-num").on("click", function () {
        var url = window.location.href;
        var cur_num = $(this).data("cur");
        if ($(this).hasClass("num-go")) {
            cur_num = ($(".page-input input").val() ? $(".page-input input").val() : 1);
        }
        if (url.indexOf("?") == -1) {
            url += "?";
        }
        if (!$.getUrlParam("curPage")) {
            url = url + '&curPage=' + cur_num;
            $(this).attr("href", url);
        } else {
            replaceParamVal("curPage", cur_num);
        }
    });

    $(".page-sel").change(function () {
        var url = window.location.href;
        var page_len = $(this).val();
        if (url.indexOf("?") == -1) {
            url += "?";
        }
        if (!$.getUrlParam("Length")) {
            url = url + '&Length=' + page_len;
            window.location.href = url;
        } else {
            replaceParamValNo("curPage", 1);
            replaceParamVal("Length", page_len);
        }
    });

}

//会议室平面图预览
function imgView(url) {
    var img_htm = '<div class="meet-img-con" style="position:fixed;width:100%;height:100%;background:rgba(0,0,0,0.6);text-align:center;vertical-align:middle;z-index:9999;top:0;left:0;">' +
        '<img class="trueimg" style="" src="' + url + '" />' +
        '<i class="meet-icon icon-close js-img-close" style="background:#fff;border:1px solid #777;padding:10px 15px;position:fixed;">X</i>' +
        '</div> ';
    $("body").append(img_htm);
    var img_ele = $(".meet-img-con").find("img");
    var icon_ele = $(".meet-img-con").find(".meet-icon");
    $("<img/>").attr("src", img_ele.attr("src")).load(function () {
        /*
        如果要获取图片的真实的宽度和高度有三点必须注意
        1、需要创建一个image对象：如这里的$("<img/>")
        2、指定图片的src路径
        3、一定要在图片加载完成后执行如.load()函数里执行
        */
        var realWidth = this.width;
        var realHeight = this.height;
        //如果真实的宽度大于浏览器的宽度就按照100%显示
        if (realWidth / img_ele.height() > $(".meet-img-con").width() / $(".meet-img-con").height()) {
            if (realHeight > $(".meet-img-con").height()) {
                img_ele.height("80%");
            }
        } else {//如果小于浏览器的宽度按照原尺寸显示
            if (realWidth > $(".meet-img-con").width()) {
                img_ele.width("80%");
            }
        }

        img_ele.css("margin-top", ($(".meet-img-con").height() - img_ele.height()) / 2 + "px");
        icon_ele.css("top", (($(".meet-img-con").height() - img_ele.height()) / 2 - 1) + "px");
        icon_ele.css("left", (($(".meet-img-con").width() - img_ele.width()) / 2 + img_ele.width() - 40) + "px");


    });

    $(".js-img-close").on("click", (function () {
        $(this).parents(".meet-img-con").remove();
    }));
}

$(function () {

    //文本框一键清除按钮显示
    $(".has-clear-input").focus(function () {
        $(this).siblings(".js-clear-input").show();
    });
    //输入框失去焦点
    $(".has-clear-input").blur(function () {
        var that = $(this);
        setTimeout(function () {
            that.siblings(".js-clear-input").hide();
        }, 200);
    });
    //输入框一键清除按钮点击
    $(".js-clear-input").click(function () {
        var inp = $(this).siblings("input.has-clear-input");
        inp.val("");
    });

    //人员机构移除操作
    $(".js-remove-span").click(function () {
        $(this).parent(".has-remove-span").remove();
    });

    //组织机构模态框
    $(".js-close-model").click(function () {
        $(this).parents(".js-model").hide();
    });

    //点击选择组织机构模态框弹出
    $(".js-sel-user").click(function () {
        var model = $(".js-user-model");
        if (model.css("display") == "none") {
            model.css({"right": "-100%"});
            model.show();
            model.animate({
                right: 0
            }, 500);
        } else {
            return false;
        }
    });


    //办公系统侧边导航展开关闭
    $(".js-nav-open").on("click", function () {
        $(this).parent(".main-nav").toggleClass("active");
        $(".main-container").toggleClass("active");
    });

    //切换查看参加不参加人员
    $(".detail-user .meet-btn").click(function () {
        var index = $(this).index();
        $(".detail-user .meet-btn").removeClass("active");
        $(this).addClass("active");
        $(".detail-user-list").find(".meet-user-span").hide();
        $(".detail-user-list").find(".meet-user-span").eq(index).show();
    });


});

