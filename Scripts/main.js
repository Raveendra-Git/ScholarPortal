
///////////////////////////////////////////////////////////
//* Pagination
//////////////////////////////////////////////////////////

//Variables For Grid
var CurPage = 1;
var LastPage = 0;
var PageSize = 10;
var TotalRows = 0;
var Page = 0;
var GdPageIndx = 1;
var OrderByCol = '';
var OrderBy = '';
var numclick = 0;

//function page events
function fnBindPageEvents(gridfunction, pagination, previousid, nextid) {
    if (previousid == undefined)
        previousid = nextid = null;
    //Grid Page  
    if (TotalRows > 0) {
        if (numclick == 0) {
            $("#" + pagination + " li").remove();
            //previous link
            $("#" + pagination).append('<li class="previous" id=' + previousid + ' onclick="fnpreviousclick(this);"><a href="#">Previous</a></li>');
            LastPage = Math.ceil(TotalRows / PageSize);
            Page = LastPage;
            var i = CurPage;
            if (CurPage >= (LastPage - GdPageIndx)) {
                i = i - GdPageIndx;
                Page = CurPage;
                if (i < 1) {
                    i = 1;
                    Page = LastPage;
                }
            }
            //var i=1;  
            while (i <= Page) {
                if (i <= parseInt(CurPage) + 4) {
                    $("#" + pagination).append('<li class="nums" id="li' + i + '" onclick="fnnumclick(this);"><a href="#">' + i + '</a></li>');
                }
                if (i == Page) {
                    $("#" + pagination + " li:eq(1)").addClass('active');
                    if (i > parseInt(CurPage) + 4) {
                        $("#" + pagination).append('<li class="nums disabled"><a href="#">..</a></li>');
                        $("#" + pagination).append('<li class="nums" id="li' + i + '" onclick="fnnumclick(this);"><a href="#">' + i + '</a></li>');
                    }
                }
                i++;
            }
            //next link
            $("#" + pagination).append('<li class="next" id=' + nextid + ' onclick="fnnextclick(this);"><a href="#">Next</a></li>');
        }
        if (previousid != null) {
            if (CurPage == 1)
                $('#' + previousid).addClass('disabled');
            else
                $('#' + previousid).removeClass('disabled');
            if (CurPage == LastPage)
                $('#' + nextid).addClass('disabled');
            else
                $('#' + nextid).removeClass('disabled');
        }
        else {
            if (CurPage == 1)
                $("#" + pagination).find('.previous').addClass('disabled'); //$('.previous').addClass('disabled');
            else
                $("#" + pagination).find('.previous').removeClass('disabled'); //$('.previous').removeClass('disabled');

            if (CurPage == LastPage)
                $("#" + pagination).find('.next').addClass('disabled'); //$('.next').addClass('disabled');
            else
                $("#" + pagination).find('.next').removeClass('disabled'); //$('.next').removeClass('disabled');
        }
    }
    else {
        $("#" + pagination + " li").remove();
        //previous link
        $("#" + pagination).append('<li class="previous disabled"><a href="#">Previous</a></li>');
        //next link
        $("#" + pagination).append('<li class="next disabled"><a href="#">Next</a></li>');
    }

    $('a[href="#"]').click(function (event) {
        event.preventDefault();
    });
}

//Grid pagesize change
function fngridpage(obj) {
    var gridfunction = $(obj).attr("gridfunction");
    PageSize = $(obj).val();
    CurPage = 1;
    numclick = 0;
    window[gridfunction]();
}

//function grid number click
function fnnumclick(obj) {
    var gridfunction = $(obj).parent().attr("gridfunction");
    var pagination = $(obj).parent().attr("id");
    if (CurPage != $(obj).text()) {
        CurPage = $(obj).text();
        $("#" + pagination + " li").removeClass(' active');
        $(obj).addClass('active');
        numclick = 1;
        window[gridfunction]();
    }
}

//Grid sorting
function fngridsorting(obj) {
    var gridfunction = $(obj).attr("gridfunction");
    if ($(obj).hasClass('sorting')) {
        $('#datatable-tabletools th.sorting_asc').removeClass('sorting_asc').addClass('sorting');
        $('#datatable-tabletools th.sorting_desc').removeClass('sorting_desc').addClass('sorting');
        $(obj).removeClass('sorting').addClass('sorting_asc');
        OrderBy = ' asc';
    }
    else if ($(obj).hasClass('sorting_asc')) {
        $(obj).removeClass('sorting_asc').addClass('sorting_desc');
        OrderBy = ' DESC';
    }
    else if ($(obj).hasClass('sorting_desc')) {
        $(obj).removeClass('sorting_desc').addClass('sorting_asc');
        OrderBy = ' ASC';
    }
    OrderByCol = $(obj).attr('sortby');
    window[gridfunction]();
}


//Pagination Next & Previous click 
function fnnextclick(obj) {
    var gridfunction = $(obj).parent().attr("gridfunction");
    var pagination = $(obj).parent().attr("id");
    if (CurPage != Page) {
        if (parseInt(CurPage) + 5 > Page) {
            numclick = 1;
            $("#" + pagination + " li").removeClass(' active');
            $("#" + pagination + " li#li" + (parseInt(CurPage) + 1) + '').addClass(' active');
        }
        else {
            numclick = 0;
            $("#" + pagination + " li").removeClass(' active');
            $("#" + pagination + " li:eq(" + CurPage + ")").addClass(' active');
        }
        CurPage = parseInt(CurPage) + 1;
        window[gridfunction]();
    }
}

function fnpreviousclick(obj) {
    var gridfunction = $(obj).parent().attr("gridfunction");
    var pagination = $(obj).parent().attr("id");
    if (CurPage != 1) {
        if (parseInt(CurPage) + 4 > Page) {
            numclick = 1;
            $("#" + pagination + " li").removeClass(' active');
            $("#" + pagination + " li#li" + (parseInt(CurPage) - 1) + '').addClass(' active');
        }
        else {
            numclick = 0;
            $("#" + pagination + " li").removeClass(' active');
            $("#" + pagination + " li:eq(" + CurPage + ")").addClass(' active');
        }
        CurPage = parseInt(CurPage) - 1;
        window[gridfunction]();
    }
}

//////////////////////////////////////////////////////////////////////////*End Pagination*////////////////////////////////////////////////////////////////////

////Function to block & unblock page
//function ShowBlockUI(control) {
//    debugger;
//    $(control).block({
//        message: '<img src="../Images/loading_blue.gif"  width="60" height="60" />',
//        css: { border: 'none', padding: '15px', backgroundColor: 'none' }
//    });
//}

//function HideBlockUI(control) {
//    debugger;
//    $(control).unblock();
//}