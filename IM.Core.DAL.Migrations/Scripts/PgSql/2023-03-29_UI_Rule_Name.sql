CREATE UNIQUE INDEX 
    if not exists ui_rule_name_sla_id
    on rule(name, sla_id)
    where operational_level_agreement_id is null;
    
CREATE UNIQUE INDEX 
    if not exists ui_rule_name_ola_id
    on rule(name, operational_level_agreement_id)
    where sla_id is null;