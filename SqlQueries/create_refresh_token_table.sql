CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
create table refresh_token(

    fk_user_id int not null,
    token varchar(255) not null unique ,
    key varchar(255) not null unique ,
    expiring_date timestamp not null,

    constraint fk_user_id
        foreign key (fk_user_id)
        references "user"(id)
        match simple
        on delete cascade
        on update cascade,
        primary key (fk_user_id,key)

)