
import { IProject } from "../interfaces/IProject";
import useFetch from "../hooks/useFetch";
import { useState } from "react";

const Project = ({ project, deleteAction }: { project: IProject, deleteAction: (id: number) => void }) => {
 
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
                        <button onClick={() => deleteAction(project.id)} className="btn">delete</button>
                    </div>
                    
                </tr>
            </tbody>
        </>
    );
}

const ProjectList = () => {
    const { data, setData, loading, error } = useFetch<IProject[] | null>('api/Project');
    const [deleteActionOccured, setDeleteActionOccured] = useState<string | null>(null);

    const deleteProject = async (id: number) => {
        const res = await fetch(`https://localhost:7172/api/Project/${id}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json"
            },
        });
        if (res.status === 204) {
            setData((prev) => (prev ? prev.filter((x) => x.id !== id) : []));
            setDeleteActionOccured("Successfully deleted project")
            return;
        }
        setDeleteActionOccured("failed to delete project")
        await new Promise((resolve) => setTimeout(() => {
            setDeleteActionOccured(null);
            resolve(null);
        }, 4000));
    }

    return (
        <div className="overflow-x-auto text-align-center">
            {deleteActionOccured &&
                <div className="toast toast-top toast-start z-10">
                    <div className="alert">
                        <span>{deleteActionOccured}</span>
                    </div>
                </div>
            }
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
                                    deleteAction={deleteProject}
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




