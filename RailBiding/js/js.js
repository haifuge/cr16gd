//配置选项
var host = "http://192.168.1.33:8088/";

$("input").focus(function () {
    $(this).attr("placeholder", "");
});
$("textarea").focus(function () {
    $(this).attr("placeholder", "");
});
//初始化icheck
function init_icheck(){
    $('input.icheck').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal',
        increaseArea: '20%' // optional
    });

    //全选
    $('#all-check').on('ifChanged', function(){
        if($(this).is(":checked")){
            $("table.meet-table .icheck").iCheck('check');
        }else{
            $("table.meet-table .icheck").iCheck('uncheck');
        }
    });
}

$.getUrlParam = function (name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
} 

//分页
function pages(cur, totalPage, totalRows, setNum) {
    if (!totalPage) return false;
    var htm = '共有' + totalRows + '条，每页显示', set_num_htm = '<select class="page-sel">';
    $(".page").html("");
    setNum = setNum ? setNum : 5;
    for (var k = 1; k < 5; k++) {
        set_num_htm += '<option value="' + k * 5 + '" ' + (setNum == k * 5 ? "selected" : "") + '>' + k * 5 + " 条/页" + '</option>'
    }
    set_num_htm = set_num_htm + '</select>条';
    htm = htm + set_num_htm;

    htm += '<a data-cur="1" class="js-page-num page-num" style="margin-left:5px;">《</a>' +
        '<a class="js-page-num page-num num-left" data-cur="' + (cur > 1 ? (cur - 1) : 1) + '">&lt;</a>';

    if (totalPage != 1) {
        htm += '<a data-cur="' + ((cur + 1) > totalPage ? (cur - 1) : cur) + '" class="js-page-num page-num clickpage ' + (((cur + 1) > totalPage ? (cur - 1) : cur) == cur ? "active" : "") + '">' + ((cur + 1) > totalPage ? (cur - 1) : cur) + '</a>';
    }

    htm += '<a data-cur="' + ((cur + 1) > totalPage ? totalPage : (cur + 1)) + '" class=" js-page-num page-num clickpage ' + (((cur + 1) > totalPage ? totalPage : (cur + 1)) == cur ? " active" : "") + '">' + ((cur + 1) > totalPage ? totalPage : (cur + 1)) + '</a>' +
        '<a class="js-page-num num-right page-num" data-cur="' + (cur < totalPage ? (cur + 1) : totalPage) + '">&gt;</a>' +
        '<a data-cur="' + totalPage + '" class="js-page-num page-num">》</a>' +
        '<input type="text" value="" class="page-num page-input"/>' +
        '<a class="page-num num-go active" id="pagego" data-cur="' + ((cur + 2) > totalPage ? cur : (cur + 2)) + '">GO</a>';
    $(".page").html(htm);

    $(".page > a.js-page-num ").on("click", function () {
        clickpagenum = $(this).html();
        if (clickpagenum == "《") {
            nowpage = 1;
        } else if (clickpagenum == "&lt;") {
            if (nowpage > 1) {
                nowpage = nowpage - 1
            }
        } else if (clickpagenum == "&gt;") {
            if (nowpage < totalPage) {
                nowpage++;
            }
        } else if (clickpagenum == "》") {
            nowpage = totalPage//最后一页;
        } else {
            nowpage = parseInt(clickpagenum);
        }

        Paginator(nowpage, page_size)
    })
    $("#pagego").on("click", function () {
        nowpage = parseInt(($("page-input").val() != "" ? $(".page-input").val() : 1));
        Paginator(nowpage, page_size)
    });

    $(".page-sel").change(function () {
        page_size = $(this).val();
        Paginator(nowpage, page_size)
    });
}
function replaceParamVal(paramName, replaceWith) {
    var oUrl = this.location.href.toString();
    var re = eval('/(' + paramName + '=)([^&]*)/gi');
    if (re.test(oUrl)) {
        var nUrl = oUrl.replace(re, paramName + '=' + replaceWith);
    } else {
        if (oUrl.indexOf('?') > 0) {
            var nUrl = oUrl + "&" + paramName + "=" + replaceWith;
        } else {
            var nUrl = oUrl + "?" + paramName + "=" + replaceWith;
        }
    }
    this.location = nUrl;
}
//会议室平面图预览
function imgView(url){
    var img_htm = '<div class="meet-img-con" style="position:fixed;width:100%;height:100%;background:rgba(0,0,0,0.6);text-align:center;vertical-align:middle;z-index:9999;top:0;left:0;">'+
        '<img class="trueimg" style="" src="'+ url +'" />'+
        '<i class="meet-icon icon-close js-img-close" style="background:#fff;border:1px solid #777;padding:10px 15px;position:fixed;">X</i>'+
        '</div> ';
    $("body").append(img_htm);
    var img_ele = $(".meet-img-con").find("img");
    var icon_ele = $(".meet-img-con").find(".meet-icon");
    $("<img/>").attr("src", img_ele.attr("src")).load(function() {
        /*
        如果要获取图片的真实的宽度和高度有三点必须注意
        1、需要创建一个image对象：如这里的$("<img/>")
        2、指定图片的src路径
        3、一定要在图片加载完成后执行如.load()函数里执行
        */
        var realWidth = this.width;
        var realHeight = this.height;
        //如果真实的宽度大于浏览器的宽度就按照100%显示
        if(realWidth/img_ele.height() > $(".meet-img-con").width()/$(".meet-img-con").height()){
            if(realHeight > $(".meet-img-con").height()){
                img_ele.height("80%");
            }
        }else{//如果小于浏览器的宽度按照原尺寸显示
            if(realWidth > $(".meet-img-con").width()){
                img_ele.width("80%");
            }
        }

        img_ele.css("margin-top",($(".meet-img-con").height()-img_ele.height())/2 +"px");
        icon_ele.css("top",(($(".meet-img-con").height()-img_ele.height())/2 - 1) +"px");
        icon_ele.css("left",(($(".meet-img-con").width()-img_ele.width())/2 + img_ele.width() - 40) +"px");


    });

    $(".js-img-close").on("click",(function(){
        $(this).parents(".meet-img-con").remove();
    }));
}

$(function(){

    //文本框一键清除按钮显示
    $(".has-clear-input").focus(function(){
        $(this).siblings(".js-clear-input").show();
    });
    //输入框失去焦点
    $(".has-clear-input").blur(function(){
        var that = $(this);
        setTimeout(function(){
            that.siblings(".js-clear-input").hide();
        },200);
    });
    //输入框一键清除按钮点击
    $(".js-clear-input").click(function(){
        var inp = $(this).siblings("input.has-clear-input");
        inp.val("");
    });

    //人员机构移除操作
    $(".js-remove-span").click(function(){
        $(this).parent(".has-remove-span").remove();
    });

    //组织机构模态框
    $(".js-close-model").click(function(){
        $(this).parents(".js-model").hide();
    });

    //点击选择组织机构模态框弹出
    $(".js-sel-user").click(function(){
        var model = $(".js-user-model");
        if(model.css("display")=="none"){
            model.css({"right":"-100%"});
            model.show();
            model.animate({
                right:0
            },500);
        }else{
            return false;
        }
    });

    //单选框、复选框
    //$('input.icheck').iCheck({
    //    checkboxClass: 'icheckbox_minimal',
    //    radioClass: 'iradio_minimal',
    //    increaseArea: '20%' // optional
    //});


    //全选
    $('#all-check').on('ifChanged', function(){
        if($(this).is(":checked")){
            $("table.meet-table .icheck").iCheck('check');
        }else{
            $("table.meet-table .icheck").iCheck('uncheck');
        }
    });

    //办公系统侧边导航展开关闭
    $(".js-nav-open").on("click",function(){
        $(this).parent(".main-nav").toggleClass("active");
        $(".main-container").toggleClass("active");
        if ($(".main-nav").width() == "170") {
            $(".leftnav-hid").css("margin-left", "315px")
            $(".leftnav-hid2").css("margin-left", "170px")
        } else {
            $(".leftnav-hid").css("margin-left", "225px")
            $(".leftnav-hid2").css("margin-left", "80px")
        }
    });

    //切换查看参加不参加人员
    $(".detail-user2 .meet-btn2").click(function(){
        var index = $(this).index();
        $(".detail-user2 .meet-btn2").removeClass("active");
        $(this).addClass("active");
        $(".detail-user-list2").find(".meet-user-span2").hide();
        $(".detail-user-list2").find(".meet-user-span2").eq(index).show();
    });
    $(".detail-user .meet-btn").click(function(){
        var index = $(this).index();
        $(".detail-user .meet-btn").removeClass("active");
        $(this).addClass("active");
        $(".detail-user-list").find(".meet-user-span").hide();
        $(".detail-user-list").find(".meet-user-span").eq(index).show();
    });



});



//验证证件编号
function checkpsd1(){

    na=form1.youbh.value;

    if( na.length <15 || na.length >15)

    {

        zjbh.innerHTML='<font class="tips_false bg01">不能超过15个字符</font>';

    }else{

        zjbh.innerHTML='<font class="tips_true bg03">填写正确</font>';


    }

}

//验证社会统一代码
function checkpsd2(){

    na=form1.youziz.value;

    if( na.length <18 || na.length >18)

    {

        zizhi.innerHTML='<font class="tips_false bg01">必须是18位社会信用代码号</font>';

    }else{

        zizhi.innerHTML='<font class="tips_true bg03">填写正确</font>';



    }



}





window.onscroll = function () {
                 var topScroll = $(document).scrollTop(); //滚动的距离,距离顶部的距离
                 console.log(topScroll)
                 if (topScroll > 80) {  //当滚动距离大于80px时执行下面的东西
    
                     $(".main-nav").css({"position":"fixed","top":"0"})
                     $(".left-nav").css({ "position": "fixed", "top": "0", "left": "auto" })
                     $(".leftnav-hid").css({ "position": "fixed", "margin-top": "-30px" })
                     $(".leftnav-hid2").css({ "position": "fixed", "margin-top": "-30px" })
             
                 }else{//当滚动距离小于250的时候执行下面的内容，也就是让导航栏恢复原状
                     $(".main-nav").css({ "position": "absolute", "top": "80px" })
                     $(".left-nav").css({ "position": "absolute", "left": "auto" })
                     $(".leftnav-hid").css({ "position": "absolute", "margin-top": "80px" })
                     $(".leftnav-hid2").css({ "position": "absolute", "margin-top": "80px" })
                 }
}

$(".main-nav ul li a").mouseover(function () {
    $(".leftnav-hid2").css("z-index","0");
})
$(".main-nav ul li a").mouseout(function () {
    $(".leftnav-hid2").css("z-index", "100");
})
var navheight = $(".main-nav").height();
$(".left-nav").css("height", navheight)


$(".leftnav-hid").click(function () {
        $(".main-con").css("padding-left", "0px");
        $(".left-nav").css({ "display": "none" });
        $(".leftnav-hid").hide()
        $(".leftnav-hid2").show();
})

$(".leftnav-hid2").click(function () {
    $(".main-con").css("padding-left", "150px");
    $(".left-nav").css({ "display": "block" });
    $(".leftnav-hid").show()
    $(".leftnav-hid2").hide();
})

