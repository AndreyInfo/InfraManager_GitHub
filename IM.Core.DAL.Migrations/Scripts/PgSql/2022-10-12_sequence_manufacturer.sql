DO $$
BEGIN
    create sequence if not exists pk_manufacturer_seq;
    alter table manufacturers alter identificator set default nextval('pk_manufacturer_seq');
    perform setval('pk_manufacturer_seq', (SELECT max(identificator)+1 from manufacturers) );
END
$$
