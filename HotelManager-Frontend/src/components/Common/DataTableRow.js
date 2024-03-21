const DataTableRow = ({ columns, row, index, handle }) => {
  return (
    <tr className="data-table-body-row" key={index}>
      {columns.map((column) => (
        <td className="data-table-body-row-cell" key={column.key}>
          {row[column.key]}
        </td>
      ))}
      {handle && (
        <td className="data-table-body-row-cell-handle">
          {handle.map((action, index) => (
            <button
              className="data-table-body-row-cell-handle-button"
              key={index}
              onClick={() => action.onClick(row)}
            >
              {action.label}
            </button>
          ))}
        </td>
      )}
    </tr>
  );
};

export default DataTableRow;
