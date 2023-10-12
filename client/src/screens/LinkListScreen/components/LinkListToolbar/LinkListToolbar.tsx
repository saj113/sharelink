import TagFilter from './TagFilter/TagFilter.tsx';
import SearchBar from './Search/SearchBar.tsx';

const LinkListToolbar = () => {
    return (
        <div className="flex flex-col">
            <div className="flex mb-3">
                <TagFilter />
            </div>
            <SearchBar />
        </div>
    );
};

export default LinkListToolbar;
