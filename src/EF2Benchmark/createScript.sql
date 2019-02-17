-- Table: public."Users"

DROP TABLE IF EXISTS public."Users";

CREATE TABLE public."Users"
(
    "Id" serial PRIMARY KEY,
    "Name" text,
    "Age" integer NOT NULL
);

ALTER TABLE public."Users"
    OWNER to postgres;