CREATE OR REPLACE FUNCTION public.aspnet_users_selectname(
	_role character varying,
	_user_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select aspnet_users_selectname(null,7)
*/
BEGIN
	RETURN (
		SELECT
			JSON_AGG ( users )
		FROM ( SELECT
        	    aspnet_users.id,
				aspnet_users.user_id,
			 	aspnet_users.name
			FROM
			 	public.aspnet_users
			 	LEFT JOIN public.aspnet_user_roles ON aspnet_user_roles.user_id = aspnet_users.id
			 	LEFT JOIN public.aspnet_roles ON aspnet_roles.id = aspnet_user_roles.role_id
			WHERE
			 	aspnet_users.is_active = true
			 	AND ( _role IS NULL OR LOWER(aspnet_roles.name) = 'staff')
                AND (_user_id IS NULL OR  aspnet_users.user_id = _user_id)
			ORDER BY
			 	aspnet_users.name ) AS users );
END
$BODY$;

ALTER FUNCTION public.aspnet_users_selectname(_role character varying, _user_id integer)
    OWNER TO postgres;
