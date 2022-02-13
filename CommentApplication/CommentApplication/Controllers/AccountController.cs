using CommentApplication.Models;
using CommentApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CommentApplication.Controllers
{
    public class AccountController : Controller
    {

        // Return Home page.
        public ActionResult Index()
        {
            return View();
        }

        //Return Register view
        public ActionResult SignUp()
        {
            return View();
        }
        //Return Forgot Password view
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //Return Forgot Password view
        public ActionResult Comments()
        {
            if (IsUserLoggedIn())
            {
                List<UserCommentViewModel> userComments = new List<UserCommentViewModel>();
                using (var dataContext = new CommentApplicationEntities())
                {
                    //Retireving the user details from DB based on username and password enetered by user.
                    userComments = dataContext.UserComments.Select(x => new UserCommentViewModel { Comment = x.Comment, CreatedTime = x.CreatedTime.ToString(), Email = x.User.Email }).OrderByDescending(x => x.CreatedTime).ToList();
                    //If user is present, then true is returned.

                }
                return View(userComments);
            }
            else
                return RedirectToAction("SignIn");
        }
        
        [HttpPost]
        public ActionResult SaveSignUpDetails(SignUpViewModel SignUpDetails)
        { 
            if (ModelState.IsValid && !IsUserAlreadyExists(SignUpDetails.Email))
            {
                //create database context using Entity framework 
                using (var databaseContext = new CommentApplicationEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    User user = new User();

                    //Save all details in RegisterUser object

                    user.Email = SignUpDetails.Email;
                    user.Password = SignUpDetails.Password;
                    user.SecretCode = SignUpDetails.SecretCode;
                    user.CreatedTime = DateTimeOffset.Now;


                    //Calling the SaveDetails method which saves the details.
                    databaseContext.Users.Add(user);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("SignUp");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("SignUp", SignUpDetails);
            }
        }

        [HttpPost]
        public ActionResult SaveUserComment()
        {            
            if (ModelState.IsValid &&  IsUserLoggedIn())
            {
                //create database context using Entity framework 
                using (var databaseContext = new CommentApplicationEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    UserComment newUserComment = new UserComment();
                    string email = Session["Email"].ToString();
                    //Save all details in RegitserUser object     

                    newUserComment.Comment = HttpContext.Request.Form["Comment"];
                    newUserComment.CreatedTime = DateTimeOffset.Now;
                    newUserComment.UserId = databaseContext.Users.Where(x => x.Email.Equals(email)).Select(x => x.Id).FirstOrDefault();


                    //Calling the SaveDetails method which saves the details.
                    databaseContext.UserComments.Add(newUserComment);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "Comments Saved";
                return RedirectToAction("Comments");
            }
            else
            {
                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return RedirectToAction("SignIn");
            }
        }

        [HttpPost]
        public ActionResult DisplayUserPassword(ForgotPasswordViewModel userDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.


            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new CommentApplicationEntities())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    UserComment newUserComment = new UserComment();
                    string email = userDetails.Email;
                    string secretCode = userDetails.SecretCode;
                    //Save all details in RegitserUser object     
                    
                    string password = databaseContext.Users.Where(x => x.Email.Equals(email) & x.SecretCode.Equals(secretCode))?.Select(x => x.Password)?.FirstOrDefault();

                    ViewBag.Message = password!=null ?"Your Password: "+ password : "Email or Secret code looks invalid";
                }

               
                return View("ForgotPassword");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("ForgotPassword", userDetails);
            }
        }

        public ActionResult SignIn()
        {
            return View();
        }

        //The SignIn form is posted to this method.
        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    Session["Email"] = model.Email;
                    return RedirectToAction("Comments");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username and password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        public bool IsUserLoggedIn()
        {             
            return Session["Email"] != null;
        }

        public bool IsUserAlreadyExists(string email)
        {
            using (var databaseContext = new CommentApplicationEntities())
            {
                return databaseContext.Users.Where(x => x.Email.Equals(email)).Any();
            }

        }
        //function to check if User is valid or not
        public User IsValidUser(SignInViewModel model)
        {
            using (var dataContext = new CommentApplicationEntities())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                User user = dataContext.Users.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel modelView)
        {
            return RedirectToAction("ForgotPassword");
        }

        [HttpPost]
        public ActionResult Comments(UserCommentViewModel modelView)
        {
            return RedirectToAction("Comments");
        }
    }

}