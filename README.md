## Action Method
    [HttpPost]
    public JsonResult Grid(KendoGridRequest request)
    {
        // build your query
        var query = _db.Employees;

        // pass the query and request to the KendoGrid Object
        var grid = new KendoGrid<Employee>(request, query);

        // serialize object
        return Json(grid);
    }


## View
    <div id="grid"></div>
    <script type="text/javascript">
        var url = '@Url.Action("Grid")';

        var dataSource = new kendo.data.DataSource({
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
            pageSize: 20,
            transport: {
                read: {
                    type: 'post',
                    dataType: 'json',
                    url: url
                }
            },
            schema: {
                data: 'data',
                total: 'total',
                model: {
                    id: 'EmployeeId',
                    fields: {
                        FirstName: { type: 'string' },
                        LastName: { type: 'string' },
                        Email: { type: 'string' }
                    }
                }
            }
        });

        $('#grid').kendoGrid({
            dataSource: dataSource,
            height: 400,
            filterable: true,
            sortable: true,
            pageable: true
        });

    </script>