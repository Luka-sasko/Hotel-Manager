// Create table depenting on the data (array of objects (table rows)),  columns (array of objects (table headers)) and handle object (arry of objects (table actions))
import DataTableRow from "./DataTableRow";

const DataTable = ({ data, columns, handle }) => {
  return (
    <table className="data-table">
      <thead className="data-table-header">
        <tr className="data-table-hader-row">
          {columns.map((column) => (
            <th className="data-table-header-row-cell" key={column.key}>
              {column.label}
            </th>
          ))}
          <th className="data-table-header-row-cell"></th>
        </tr>
      </thead>
      <tbody className="data-table-body">
        {data.map((row, index) => (
          <DataTableRow
            key={index}
            columns={columns}
            row={row}
            index={index}
            handle={handle}
          />
        ))}
      </tbody>
    </table>
  );
};

export default DataTable;
