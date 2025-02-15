import { IProject } from "../interfaces/IProject";


const ProjectItem = ({ project, detailsAction, deleteAction, updateAction }:
    {
        project: IProject,
        detailsAction: (project: IProject) => void,
        deleteAction: (id: number) => void,
        updateAction: (project: IProject) => void
    }) => {

    return (
        <>
            <tbody>
                <tr>
                    <td>{project.id}</td>
                    <td>{project.name}</td>
                    <td>{project.startTime}</td>
                    <td>{project.endTime}</td>
                    <td>{project.status}</td>
                    <td>
                        <button onClick={() => detailsAction(project)} className="btn btn-accent btn-xs mr-1">Details</button>
                        <button onClick={() => updateAction(project)} className="btn btn-xs btn-accent mr-1">update</button>
                        <button onClick={() => deleteAction(project.id)} className="btn btn-error btn-xs">delete</button>
                    </td>
                </tr>
            </tbody>
        </>
    );
}
export default ProjectItem;