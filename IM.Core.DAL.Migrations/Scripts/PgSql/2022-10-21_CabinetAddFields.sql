DO $$
    BEGIN
        alter table product_catalog_category alter column id set default gen_random_uuid();
        alter table product_catalog_type alter column id set default gen_random_uuid();
        drop index if exists pcc_name_parent;
        create unique index pcc_name_parent on product_catalog_category(name, coalesce(parent_product_catalog_category_id,'00000000-0000-0000-0000-000000000000')) where removed = false;
        create unique index if not exists pct_name_parent on product_catalog_type(name, product_catalog_category_id) where removed = false;

        insert into product_catalog_category (name, removed) values ('Шкафы',false)
            on conflict do nothing;
        
        insert into product_catalog_template (id, name, parent_id, class_id)
            values (421, N'Шкафы', null, 4) on conflict do nothing;
        
        insert into product_catalog_type (name, removed,product_catalog_template_id,product_catalog_category_id,external_code,external_name,can_buy)
            values ('Шкафы',false,421,(SELECT id FROM product_catalog_category c where c.name ='Шкафы' and c.parent_product_catalog_category_id is null and not c.removed), '','',false )
            on conflict do nothing;
        
        
        IF NOT EXISTS(SELECT 0 --нет product_catalog_type_id
                        FROM information_schema.columns
                        WHERE table_name='cabinet_types'
                            and column_name='product_catalog_type_id' 
                        limit 1)
        THEN
            IF EXISTS(SELECT 0 -- есть productcatalogtypeid
                          FROM information_schema.columns
                          WHERE table_name='cabinet_types'
                            and column_name='productcatalogtypeid'
                          limit 1)
            THEN
                alter table cabinet_types rename column productcatalogtypeid to product_catalog_type_id;
            ELSE
                alter table cabinet_types
                    add if not exists product_catalog_type_id uuid;  
            END if;
        END IF;
        
        alter table cabinet_types
            drop constraint  if exists  fk_cabinet_types_product_catalog_type,
            add constraint fk_cabinet_types_product_catalog_type
                        foreign key (product_catalog_type_id)
                            references product_catalog_type(id)
                            ON DELETE CASCADE;

        update cabinet_types
        set product_catalog_type_id=
                (select id from product_catalog_type
                 where product_catalog_category_id =(select id from product_catalog_category c where c.name='Шкафы' and c.parent_product_catalog_category_id is null and not c.removed))
        where product_catalog_type_id is null ;

        alter table cabinet_types alter column product_catalog_type_id set not null;
        
        create unique index if not exists ux_name on cabinet_types(name,product_catalog_type_id);

        create sequence if not exists seq_cabinet_types;

        alter table cabinet_types alter identificator set default nextval('seq_cabinet_types');

        perform setval('seq_cabinet_types', (SELECT max(identificator)+1 from cabinet_types));

        ALTER TABLE cabinet_types ADD if not exists external_id VARCHAR(250) NOT NULL DEFAULT N'Шкаф';
    END
$$
