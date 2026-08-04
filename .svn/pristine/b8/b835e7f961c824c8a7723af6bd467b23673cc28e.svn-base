CREATE OR REPLACE FUNCTION public.case_property_selectall(
	_id integer,
	_case_id integer)
    RETURNS jsonb
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
/*
    select case_property_selectall(1,1)
*/
BEGIN
RETURN (
        SELECT JSON_AGG(property_data)
        FROM (
            SELECT
                case_property.id,
                case_property.case_id,
                case_property.property_type_id,
                property_type_mas.name AS property_type,
                case_property.name,
                case_property.distance,
                case_property.size_sqft,
                case_property.address,
                case_property.city,
                case_property.state_id,
                case_property.pincode,
                case_property.latitude,
                case_property.longitude,
                case_property.location,
                case_property.valuation_amount,
                case_property.is_final,
                case_property.report_doc_path,
                case_property.report_pdf_path,
                case_property.report_updated_date,
                case_property.note,
                case_property.created_by,
                case_property.created_date,
                case_property.modified_by,
                case_property.modified_date,

                -- Case Info
                "case".id,
                "case".case_id AS case_uid,
                "case".customer_name,
                "case".customer_contact,

                -- State Name
                state_mas.name AS state_name,

                -- Documents grouped by document type
                (
                    SELECT JSON_BUILD_OBJECT(

                        'bank_documents', (
                            SELECT JSON_AGG(JSON_BUILD_OBJECT(
                                'id',           case_document.id,
                                'doc_type',     case_document.doc_type,
                                'title',        case_document.title,
                                'file_path',    case_document.file_path,
                                'created_by',   case_document.created_by,
                                'created_date', case_document.created_date,
                                'modified_by',  case_document.modified_by,
                                'modified_date',case_document.modified_date
                            ))
                            FROM public.case_document
                            WHERE case_document.property_id = case_property.id
                            AND case_document.doc_type = 1
                        ),

                        'property_documents', (
                            SELECT JSON_AGG(JSON_BUILD_OBJECT(
                                'id',           case_document.id,
                                'doc_type',     case_document.doc_type,
                                'title',        case_document.title,
                                'file_path',    case_document.file_path,
                                'created_by',   case_document.created_by,
                                'created_date', case_document.created_date,
                                'modified_by',  case_document.modified_by,
                                'modified_date',case_document.modified_date
                            ))
                            FROM public.case_document
                            WHERE case_document.property_id = case_property.id
                            AND case_document.doc_type = 2
                        ),

                        'visit_sheet', (
                            SELECT JSON_AGG(JSON_BUILD_OBJECT(
                                'id',           case_document.id,
                                'doc_type',     case_document.doc_type,
                                'title',        case_document.title,
                                'file_path',    case_document.file_path,
                                'created_by',   case_document.created_by,
                                'created_date', case_document.created_date,
                                'modified_by',  case_document.modified_by,
                                'modified_date',case_document.modified_date
                            ))
                            FROM public.case_document
                            WHERE case_document.property_id = case_property.id
                            AND case_document.doc_type = 3
                        ),

                        'case_documents', (
                            SELECT JSON_AGG(JSON_BUILD_OBJECT(
                                'id',           case_document.id,
                                'doc_type',     case_document.doc_type,
                                'title',        case_document.title,
                                'file_path',    case_document.file_path,
                                'created_by',   case_document.created_by,
                                'created_date', case_document.created_date,
                                'modified_by',  case_document.modified_by,
                                'modified_date',case_document.modified_date
                            ))
                            FROM public.case_document
                            WHERE case_document.property_id = case_property.id
                            AND case_document.doc_type = 4
                        )

                    )
                ) AS documents

            FROM public.case_property

            LEFT JOIN public."case" ON case_property.case_id = "case".id
            LEFT JOIN public.state_mas ON case_property.state_id = state_mas.id
            LEFT JOIN public.property_type_mas ON case_property.property_type_id = property_type_mas.id 
            WHERE
                (_id IS NULL OR case_property.id = _id)
                AND (_case_id IS NULL OR case_property.case_id = _case_id)

            ORDER BY case_property.created_date DESC

        ) AS property_data
    );
END;

$BODY$;

ALTER FUNCTION public.case_property_selectall(_id integer, _case_id integer)
    OWNER TO postgres;
