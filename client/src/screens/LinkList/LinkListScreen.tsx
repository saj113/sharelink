import LinkListToolbar from './components/LinkListToolbar/LinkListToolbar.tsx';
import LinkInfiniteScrollList from './components/LinkInfiniteScrollList.tsx';

const LinkListScreen = () => {
    return (
        <>
            <div className="flex items-center justify-center md:mb-16 mb-8 w-full max-w-screen-sm">
                <LinkListToolbar />
            </div>

            <LinkInfiniteScrollList />
        </>
    );
};

export default LinkListScreen;