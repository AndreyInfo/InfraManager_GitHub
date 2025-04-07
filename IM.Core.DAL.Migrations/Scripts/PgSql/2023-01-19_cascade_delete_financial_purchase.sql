ALTER TABLE im.work_order_finance_purchase DROP CONSTRAINT IF EXISTS fk_work_order_finance_purchase_work_order_id;

ALTER TABLE im.work_order_finance_purchase
    ADD CONSTRAINT fk_work_order_finance_purchase_work_order_id FOREIGN KEY (work_order_id)
    REFERENCES im.work_order (id) MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE;