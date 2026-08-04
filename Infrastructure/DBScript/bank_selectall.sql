CREATE OR REPLACE FUNCTION public.bank_selectall(
	_id integer,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select public.bank_selectall(null, null);
    select public.bank_selectall(1, true);
*/
BEGIN
    RETURN (
        SELECT JSON_AGG(bank_detail)
        FROM (
            SELECT
                bank.id,
                bank.name,
                bank.code,
                bank.template_name,
                bank.template_type,
                bank.template_path,
                bank.is_active,
                bank.created_by,
                bank.created_date,
                bank.modified_by,
                bank.modified_date
            FROM public.bank
            WHERE
                (_id IS NULL OR bank.id = _id)
                AND (_is_active IS NULL OR bank.is_active = _is_active)
            ORDER BY bank.name
        ) AS bank_detail
    );
END;
$BODY$;

ALTER FUNCTION public.bank_selectall(_id integer, _is_active boolean)
    OWNER TO postgres;
