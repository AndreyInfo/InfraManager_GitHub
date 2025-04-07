CREATE UNIQUE INDEX 
    if not exists ui_login_users on users(login_name)
    where login_name != ''
        and login_name is not null;

CREATE UNIQUE INDEX
    if not exists ui_email_users on users(email)
    where email != ''
        and email is not null;