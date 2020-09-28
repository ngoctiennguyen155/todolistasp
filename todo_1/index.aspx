<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="todo_1.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta charset="utf-8">

    <link href="Css/bootstrap-4.3.1.css" rel="stylesheet" />
    <link href="Css/font-awesome.css" rel="stylesheet" />
    <link href="Css/StyleSheet1.css" rel="stylesheet" />
</head>
<body >
   
    <asp:Label ID="label1" runat="server" ><%=title%></asp:Label>

    <div class="container-fluid bg-nen">

        <div class="modal" id="myModal">
  <div class="modal-dialog">
    <div class="modal-content">

      <!-- Modal Header -->
      <div class="modal-header">
        <h4 class="modal-title">Modal Heading</h4>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>

      <!-- Modal body -->
      <div class="modal-body">
        Modal body..
      </div>

      <!-- Modal footer -->
      <div class="modal-footer">
        <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
      </div>

    </div>
  </div>
</div>

        <!--sidebar-->
    	<nav id="sidebar" class="bg-light navbar-light">
            <div id="dismiss" class="btn-danger">
                <i class="fa fa-arrow-left"></i>
            </div>

            <div class="sidebar-header">
                <h4>Công việc công khai</h4>
            </div>

            <ul class="list-unstyled components pre-scrollable">
                <!--<li>
                    <div class="card-body bg-white public-act mb-1">
                        <h4 class="card-title">Làm báo cáo</h4>
                        <p class="card-text p-0">Some example text. Some example text.</p>
                        <a href="#" class="card-link">Card link</a>
                        <a href="#" class="card-link">Another link</a>
                      </div>
                </li>
                -->
               
                <%if (jobPublicData.Rows.Count > 0) {
                        for(int i = 0;i<jobPublicData.Rows.Count;i++)
                        {
                            %>
                            <li>
                                <div class="card-body bg-white public-act mb-1">
                                    <h4 class="card-title"><%= jobPublicData.Rows[i][1] %></h4>
                                    <span>Start<p class="card-text p-0"> <%= jobPublicData.Rows[i][2].ToString().Split(' ')[0] %></p></span>
                                     <span>End<p class="card-text p-0"><%= jobPublicData.Rows[i][3].ToString().Split(' ')[0] %> </p></span>
                                    <%if (contactjobpublic[(int)jobPublicData.Rows[i][0], 0] != null)
                                            {
                                                int z = 0;
                                                while (contactjobpublic[(int)jobPublicData.Rows[i][0], z] != null)
                                                {%>
                                                    <p> <%=contactjobpublic[(int)jobPublicData.Rows[i][0], z++] %></p>
                                                    
                                             <% }
                                      }%>
                                  </div>
                            </li>
                    <%    }

                        } %>
                
                
                <!--den khu nay-->

            </ul>
        </nav>
    <!--sidebar-->

        <!--navbar header fixtop-->
        <div class="navbar fixed-top bg-info navbar-dark navbar-expand-lg ">
			<div class="container">
			<button class="navbar-toggler" data-target="#menu" data-toggle="collapse">
				<span class="navbar-toggler-icon"></span>
			</button>
			<div class="navbar-collapse collapse" id="menu">
                <button type="button" id="sidebarCollapse" class="btn btn-info p-0 public">
                        <i class="fa fa-align-left"></i>
                        <span>Công việc công khai</span>
                </button>
                <a href="#" class="navbar-brand ml-auto"> <img src="Images/Logo.png"> </a>
				<ul class="navbar-nav ml-auto" >
					<li class="nav-item active"><a href="#" class="nav-link">Trang chủ</a></li>
					<li class="nav-item"><a href="#" class="nav-link login" data-toggle="modal" data-target="#login"><i class="fa fa-user-circle icon-login"></i> Thông tin cá nhân</a></li>
					
				</ul>
			</div>
				</div>
		</div>
        <!--end navbar header top-->
        
        
        <!--content-->
        <div class="container">
            <div class="row">
                <div class="col-12">
                    <form class="form-inline mt-5" style="width: 500px" action="index.aspx" method="get">
                        <span><input class="form-control mr-1" type="date"  name="daysearch" value="<%=dbcon.Getcurrentday() %>"  /></span>
                        <button class="btn btn-info">Tìm kiếm</button>
                    </form>
                </div>
                
                
                <% string[] Weeks = new string[10];
                    Weeks[0] = "Monday";
                    Weeks[1] = "Tuesday";
                    Weeks[2] = "Wednesday";
                    Weeks[3] = "Thursday";
                    Weeks[4] = "Friday";
                    Weeks[5] = "Saturday";

                    for (int i = 0; i <6; i++)
                    {
                        DateTime dtime = new DateTime();%>
                    <div class="col-xl-4 col-lg-6 col-md-6 col-sm-12 col-12">
                    <div class="day">
                    	
                            <h4 class="tittle">Thứ <%= i+2 %></h4>
                            
                           <%for (int j = 0; j < dt.Rows.Count; j++) {
                                   dtime = (DateTime)dt.Rows[j][2];
                            
                                   if(dtime.DayOfWeek.ToString().Equals(Weeks[i]))
                                   {%>
                                        <div class="bg-white">
                                <div class="activity">
                                	<div class="form-inline">
                                        <form action="index.aspx" method="get" class="form-sua form-inline w-auto">
                                        <input type="text" class="jobtitle" name="idupdate" value="<%= dt.Rows[j][0] %>" hidden />
                                        <span class="ok"><input style="cursor: pointer" name="textupdate" class="text-edit form-control" type="text" disabled value="<%= dt.Rows[j][1] %>"></span>
                                        <button hidden><i class="fa fa-plus"></i></button>
                                            
                                        <div class="bg-edit ml-auto">
                                            <span style="padding: 3.5px; cursor:pointer; padding-top: 3px" class="edit btn-primary" ><i class="fa fa-edit"></i></span>
                                        </div>
                                            </form>
                                        <div class="bg-hover d-inline ml-auto" id="<%= dt.Rows[j][0] %>">
                                            <form action="index.aspx" method="get" >
                                                <input  name="iddelete" value="<%= dt.Rows[j][0] %>" hidden/>
                                        	    <button runat="server" class="edit btn-danger"><i class="fa fa-close"></i> </button>
                                            </form>
                                          </div>
                                    </div>
                                
                                	<ul class="form-inline">
                                     
                                      <%if (contacts[(int)dt.Rows[j][0], 0] != null)
                                            {
                                                int z = 0;
                                                while (contacts[(int)dt.Rows[j][0], z] != null)
                                                {%>
                                                    
                                                        
                                                        <form action="index.aspx" method="get" >
                                                            <li class="btn btn-info"><%=contacts[(int)dt.Rows[j][0],z] %>
                                                                <input type="text" name="namecontact" value="<%=idcontacts[(int)dt.Rows[j][0],z++] %>" hidden/>
                                                                <input type="text" name="namecontact_idjob" value="<%=dt.Rows[j][0] %>" hidden/>
                                                                <button class="close">&times;</button>
                                                        </li>
                                                                </form>

                                                    
                                             <% }
                                      }%>
                                       
                                      <li class="input-them item-add btn ">
                                          <form  action="index.aspx" method="get">
                                             <input name="contact" class="add-member form-control" type="text" placeholder="abc@gmail.com">
                                              <input type="text" name="idjob" value="<%=dt.Rows[j][0] %>" hidden />
                                               <button hidden><i class="fa fa-plus"></i></button>
                                              
                                          </form>
                                          </li>
                                        <li class="btn btn-danger btn-them"> 
                                      	<i class="fa fa-plus"></i> 
                                      </li>
                                    </ul>
                                         <%if (comments[(int)dt.Rows[j][0], 0] != null)
                                            {
                                                int z = 0;
                                                while (comments[(int)dt.Rows[j][0], z] != null)
                                                {%>
                                                  <input type="text" value="<%=comments[(int)dt.Rows[j][0],z] %>" />
                                                    <h4><%=idnvcomments[(int)dt.Rows[j][0],z] %></h4>
                                                    <h4><%=namenvcomments[(int)dt.Rows[j][0],z++] %></h4>
                                             <% }
                                      }%>
                                    </div>
                            </div>
                                  <% }
                               } %>
                     

                            <form method="get" action="index.aspx"  >
                            <div id="demo"  class="text-add">
                               <input name="job" class="form-control" placeholder="Nhập công việc mới" ></input>
                               <input name="day" value="<%=Weeks[i] %>" hidden></input>
                                <button hidden><i class="fa fa-plus"></i></button>
                                
                            </div>
                            <span  class="btn add-activity">+ Thêm công việc mới</span>
                    </form>
                                </div>
                </div>
                <%}%>
                    
                           
        <!--end content-->
    </div>
    </div>
    </div>
    
    
    <script src="Scripts/jquery-3.5.1.min.js"></script>
    <script src="Scripts/popper.min.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script src="Scripts/jquery.mCustomScrollbar.concat.min.js"></script>
    <script>
        $(document).ready(function () {


            $(".input-them").slideUp(0);
            $(".text-add").slideUp(0);

            //var date = new Date();
            //var day = date.getDate();
            //var month = date.getMonth() + 1;
            //var year = date.getFullYear();

            //if (month < 10) month = "0" + month;
            //if (day < 10) day = "0" + day;

            //var today = year + "-" + month + "-" + day;
            //$("#ngay").attr("value", today);




            $(".ok").click(function () {
                $("#myModal").modal();
                var idjob = $(this).siblings("input").val();
                var jobtitle = $(this).siblings("input .jobtitle").val();

                console.log(jobtitle);
            });







        });

        $("#sidebar").mCustomScrollbar({
            theme: "minimal"
        });

        $('#dismiss, .overlay').click(function () {
            $('#sidebar').removeClass('active');
            $('.overlay').removeClass('active');
        });

        $('#sidebarCollapse').click(function () {
            $('#sidebar').addClass('active');
            $('.overlay').addClass('active');
            $('.collapse.in').toggleClass('in');
            $('a[aria-expanded=true]').attr('aria-expanded', 'false');
        });


        $(".bg-edit").click(function () {
            a = $(this).siblings(".ok");
            b = $(a).children(".text-edit");
            b.prop("disabled", false);
            b.focus();
        });
        $(".bg-hover").click(function () {
            //alert(typeof());
      
            a = $(this).parents(".bg-white");
            a.css("display", "none");
            
 
        });

        var closebtns = document.getElementsByClassName("close");
        var i;

        for (i = 0; i < closebtns.length; i++) {
            closebtns[i].addEventListener("click", function () {
                this.parentElement.style.display = 'none';
             
            });
        }
        $(".btn-them").click(function () {
            b = $(this).siblings(".input-them");
            b.slideToggle();
        });
        $(".add-activity").click(function () {
            b = $(this).siblings(".text-add");
            b.slideToggle();
        });
        
    </script>
    
</body>
</html>
