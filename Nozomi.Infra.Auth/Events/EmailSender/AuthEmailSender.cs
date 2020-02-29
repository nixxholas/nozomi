using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Preprocessing.Abstracts;
using Nozomi.Preprocessing.Events.Interfaces;
using Nozomi.Repo.BCL.Repository;
using Nozomi.Repo.Data;

namespace Nozomi.Infra.Auth.Events.EmailSender
{
    public class AuthEmailSender : BaseEvent<AuthEmailSender, NozomiDbContext>, IAuthEmailSender
    {
        private readonly IEmailSender _emailSender; 
        
        public AuthEmailSender(ILogger<AuthEmailSender> logger, IUnitOfWork<NozomiDbContext> unitOfWork, IEmailSender emailSender) 
            : base(logger, unitOfWork)
        {
            _emailSender = emailSender;
        }

        public Task SendEmailConfirmationAsync(string email, string callbackUrl)
        {
            string message = $@"
                <!DOCTYPE html>
                <head>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                </head>
                <body style='display: flex; color: #808285;'>
                    <div style='width: 600px; margin-left: auto; margin-right: auto; border: 1px solid #dadadb;'>
                        <div style='width: 100%; background-color: #7957d5; display: block;'>
                            <img src='https://nozomi.one/assets/logo-white.png' style='width: 275px; height: auto; display: flex; margin-left: auto; margin-right: auto;' />
                        </div>

                        <div style='padding-left: 2vw; padding-right: 2vw; padding-bottom: 2vw; text-align: center;'>
                            <p style='color: #7957d5; font-size: 2em; margin-bottom: 0;'>Hello!</p>
                            <p>
                                Welcome to Nozomi! <br />
                                To confirm your email address, click the link below:
                            </p>
                            <a href='{callbackUrl}' style='display: inline-block; margin-top: 5px; background-color: #7957d5; color: #ffffff; padding: 15px 30px 15px 30px; text-decoration: none;'>
                                Verify Email
                            </a>
                        </div>
                    </div>
                </body>
                </html>
            ";

            return _emailSender.SendEmailAsync(email, "Nozomi", message);
        }

        public Task SendPasswordResetLinkAsync(string email, string callbackUrl)
        {
            string message = $@"
                <!DOCTYPE html>

                <head>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                </head>

                <body style='display: flex; color: #808285;'>
                    <div style='width: 600px; margin-left: auto; margin-right: auto; border: 1px solid #dadadb;'>

                        <div
                            style='width: 100%; background-color: #7957d5; display: block;'>
                            <img src='https://nozomi.one/assets/logo-white.png' style='width: 275px; height: auto; display: flex; margin-left: auto; margin-right: auto;' />
                        </div>

                        <div style='padding-left: 2vw; padding-right: 2vw; padding-bottom: 2vw; text-align: center;'>

                            <p style='color: #7957d5; font-size: 2em; margin-bottom: 0;'>Forgot your password?</p>
                            <p>
                                That's okay, it happens! Click on the button <br />
                                below to reset your password.
                            </p>
                            <a href='{callbackUrl}'
                                style='display: inline-block; margin-top: 5px; background-color: #7957d5; color: #ffffff; padding: 15px 30px 15px 30px; text-decoration: none;'>
                                Reset your password
                            </a>
                            <p style='margin-top: 30px;'>
                                If you did not request this, it is possible that someone else is trying to access the Nozomi Account {email}. <strong>Do not forward or give this email to anyone.</strong>
                            </p>

                        </div>
                    </div>
                </body>

                </html>
            ";

            return _emailSender.SendEmailAsync(email, "Nozomi", message);
        }
    }
}