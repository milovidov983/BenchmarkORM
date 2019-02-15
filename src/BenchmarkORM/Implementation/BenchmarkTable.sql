-- Postgresql

-- Table: public."Entities"

-- DROP TABLE public."Entities";

CREATE TABLE public."Entities"
(
    doubledata double precision NOT NULL,
    id integer NOT NULL DEFAULT nextval('"Entities_id_seq"'::regclass),
    intdata integer NOT NULL,
    stringdata "char"[],
    CONSTRAINT "Entities_pkey" PRIMARY KEY (doubledata)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Entities"
    OWNER to postgres;