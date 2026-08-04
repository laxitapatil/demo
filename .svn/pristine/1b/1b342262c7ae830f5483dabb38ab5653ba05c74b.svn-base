CREATE OR REPLACE FUNCTION public.bank_vertical_selectall(
	_id integer,
	_branch_id integer,
	_bank_id integer,
	_is_active boolean)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select public.bank_vertical_selectall(null, null, null, null);
    select public.bank_vertical_selectall(1, null, null, true);
    select public.bank_vertical_selectall(null, 1, null, null);
    select public.bank_vertical_selectall(null, null, 1, null);
*/
BEGIN
    RETURN (
        SELECT JSON_AGG(bank_vertical_detail)
        FROM (
            SELECT
                bank_vertical.id,
                bank_vertical.branch_id,
                bank_branch.name as branch,

                bank_branch.bank_id,
                bank.name as bank,

                bank_vertical.name,
                bank_vertical.is_active,
                bank_vertical.modified_by,
                bank_vertical.modified_date
            FROM public.bank_vertical
            INNER JOIN public.bank_branch ON bank_vertical.branch_id = bank_branch.id
            INNER JOIN public.bank ON bank_branch.bank_id = bank.id
            WHERE
                (_id IS NULL OR bank_vertical.id = _id)
                AND (_branch_id IS NULL OR bank_vertical.branch_id = _branch_id)
                AND (_bank_id IS NULL OR bank_branch.bank_id = _bank_id)
                AND (_is_active IS NULL OR bank_vertical.is_active = _is_active)
            ORDER BY bank.name, bank_branch.name, bank_vertical.name
        ) AS bank_vertical_detail
    );
END;
$BODY$;

ALTER FUNCTION public.bank_vertical_selectall(_id integer, _branch_id integer, _bank_id integer, _is_active boolean)
    OWNER TO postgres;
