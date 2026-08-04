CREATE OR REPLACE FUNCTION public.rpt_case(
	_from_date timestamp without time zone,
	_to_date timestamp without time zone,
	_bank_id integer,
	_branch_id integer,
	_case_status_id integer,
	_visitor_id integer,
	_reporter_id integer,
	_case_type integer,
	_billing_month timestamp without time zone,
	_search_text character varying,
	_id integer,
	_case_id character varying)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$

/*
SELECT rpt_case(null,null,null,null,null,null,null,null,null,null,null,null)
*/

BEGIN
    RETURN (
    SELECT jsonb_build_object(
        'total_cases', COUNT(DISTINCT case_data.id),
        'total_fees', COALESCE(SUM(DISTINCT case_data.fees),0),
        'data', JSONB_AGG(case_data)
    )
    FROM (
        SELECT
            "case".id,
            "case".case_id,
            "case".created_date,
            "case".bill_date,
            "case".fees,
            "case".customer_name,
            "case".customer_contact,
            "case".visit_date,
            "case".submit_date,
            "case".bank_case_id,
            ("case".submit_date::date - "case".created_date::date) AS days,
            "case".case_status_id,
            case_status_mas.name AS case_status,
            case_property.address,
            (case_property.latitude || ', ' || case_property.longitude) AS latitude,
            case_property.distance,
            case_property.size_sqft,
            case_property.property_type_id,
            property_type_mas.name AS property_type,
            bank.name AS bank,
            bank_branch.name AS branch,
            visitor.name AS visitor_name,
            report_maker.name AS report_maker_name,
            case_type_mas.name AS case_type_mas

        FROM public."case"


            LEFT JOIN public.case_property ON case_property.case_id = "case".id
            LEFT JOIN public.case_type_mas ON "case".case_type = case_type_mas.id
            LEFT JOIN public.case_status_mas ON "case".case_status_id = case_status_mas.id
            LEFT JOIN public.bank ON "case".bank_id = bank.id
            LEFT JOIN public.bank_branch ON "case".bank_branch_id = bank_branch.id
            LEFT JOIN public.aspnet_users visitor ON "case".visitor_id = visitor.user_id
            LEFT JOIN public.aspnet_users report_maker ON "case".report_maker_id = report_maker.user_id
            LEFT JOIN public.property_type_mas ON case_property.property_type_id = property_type_mas.id

            WHERE
                (_from_date IS NULL OR "case".created_date >= _from_date)
                AND (_to_date IS NULL OR "case".created_date < (_to_date + INTERVAL '1 day'))
                AND (_bank_id IS NULL OR "case".bank_id = _bank_id)
                AND (_branch_id IS NULL OR "case".bank_branch_id = _branch_id)
                AND (_case_status_id IS NULL OR "case".case_status_id = _case_status_id)
                AND (_visitor_id IS NULL OR "case".visitor_id = _visitor_id)
                AND (_reporter_id IS NULL OR "case".report_maker_id = _reporter_id)
                AND (_case_type IS NULL OR "case".case_type = _case_type)
                AND (
                    _billing_month IS NULL OR
                    DATE_TRUNC('month', "case".bill_date) = DATE_TRUNC('month', _billing_month)
                )
                AND (_id IS NULL OR "case".id = _id)
                AND (_case_id IS NULL OR "case".case_id = _case_id)
                AND (
                    _search_text IS NULL
                    OR _search_text = ''
                    OR "case".customer_name ILIKE '%' || _search_text || '%'
                    OR "case".customer_contact ILIKE '%' || _search_text || '%'
                )

            ORDER BY "case".created_date DESC
        ) AS case_data
    );
END;
$BODY$;

ALTER FUNCTION public.rpt_case(_from_date timestamp without time zone, _to_date timestamp without time zone, _bank_id integer, _branch_id integer, _case_status_id integer, _visitor_id integer, _reporter_id integer, _case_type integer, _billing_month timestamp without time zone, _search_text character varying, _id integer, _case_id character varying)
    OWNER TO postgres;
