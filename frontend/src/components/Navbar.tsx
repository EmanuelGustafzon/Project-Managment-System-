

const Navbar = ({ createProject }: { createProject: () => void}) => {
    return (
        <div className="navbar bg-base-100 w-full">
            <div className="navbar-start">
                <a className="btn btn-ghost text-xl">Project Manager</a>
            </div>
            <div className="navbar-end">
                <button onClick={createProject} className="btn">create project</button>
            </div>
        </div>
    )
}

export default Navbar;