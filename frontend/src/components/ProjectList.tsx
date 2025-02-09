
const ProjectList = () => {
    return (
        <div className="overflow-x-auto">
            <table className="table table-xs table-pin-rows table-pin-cols">
                <thead>
                    <tr>
                        <th></th>
                        <td>Name</td>
                        <td>StartDate</td>
                        <td>EndDate</td>
                        <td>Status</td>
                        <td>TotalPrice</td>
                        <td>ProjectManager</td>
                        <td>Service</td>
                        <td>Customer</td>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <th>1</th>
                        <td>Cool project</td>
                        <td>12 dec</td>
                        <td>2 jan</td>
                        <td>ongoing</td>
                        <td>12000</td>
                        <td>Nicklas Svensson</td>
                        <td>Web Design</td>
                        <td>Lilla bokbolaget</td>
                    </tr>
                </tbody>
            </table>
        </div>
    )
}

export default ProjectList;