
import { IProject } from "../interfaces/IProject";
import useFetch from "../hooks/useFetch";

const Project = ({ project }: { project: IProject }) => {

    const deleteProject  = async (id: number) => {
        const res = await fetch(`https://localhost:7172/api/Project/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
        });
    }
    return (
        <>
            <tbody>
                <tr>
                    <td>{project.name}</td>
                    <td>{project.status}</td>
                    <td>{project.totalPrice}</td>
                    <td>{`${project.projectManager.firstName} ${project.projectManager.lastName}`}</td>
                    <td>{project.service.name}</td>
                    <td>{project.customer.name}</td>
                    <div>
                        <button className="btn">update</button>
                        <button onClick={() => deleteProject(project.id)} className="btn">delete</button>
                    </div>
                    
                </tr>
            </tbody>
        </>
    );
}

const ProjectList = () => {
    const { data, loading, error } = useFetch<IProject[] | null>('api/Project');

    return (
        <div className="overflow-x-auto">
            {error && <p>...</p>}
            {loading && <p>LOADING...</p> }
            { data &&
                <table className="table table-xs table-pin-rows table-pin-cols">
                    <thead>
                        <tr>
                            <td>Name</td>
                            <td>Status</td>
                            <td>TotalPrice</td>
                            <td>ProjectManager</td>
                            <td>Service</td>
                            <td>Customer</td>
                        </tr>
                    </thead>
                    {
                        data && data.map((project: IProject) => {
                            return (
                                <Project
                                    key={project.id}
                                    project={project}
                                />
                            );
                        })
                    }
                </table>
            }
            
        </div>
    )
}

export default ProjectList;




