CREATE OR REPLACE FUNCTION public.bank_branch_selectall(
	_id integer,
	_bank_id integer,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select public.bank_branch_selectall(null, null, null);
    select public.bank_branch_selectall(1, null, true);
    select public.bank_branch_selectall(null, 1, null);
*/
BEGIN
    RETURN (
        SELECT JSON_AGG(bank_branch_detail)
        FROM (
            SELECT
                bank_branch.id,
                bank_branch.name,

                bank_branch.bank_id,
                bank.name as bank,

                bank_branch.is_active,
                bank_branch.modified_by,
                bank_branch.modified_date
                
            FROM public.bank_branch
            INNER JOIN public.bank ON bank_branch.bank_id = bank.id
            WHERE
                (_id IS NULL OR bank_branch.id = _id)
                AND (_bank_id IS NULL OR bank_branch.bank_id = _bank_id)
                AND (_is_active IS NULL OR bank_branch.is_active = _is_active)
            ORDER BY bank.name, bank_branch.name
        ) AS bank_branch_detail
    );
END;
$BODY$;

ALTER FUNCTION public.bank_branch_selectall(_id integer, _bank_id integer, _is_active boolean)
    OWNER TO postgres;
