﻿import FloatingInput from '../../../../../components/FloatingInput.tsx';
import FloatingInputWrapper from '../../../../../components/FloatingInputWrapper.tsx';
import Button from '../../../../../components/Button.tsx';
import SearchIcon from '../../../../../components/Icons/SearchIcon.tsx';
import {useLinkStore} from '../../../../../contexts/AppContext.tsx';
import useDebounce from '../../../../../hooks/useDebounce.ts';
import {observer} from 'mobx-react-lite';

const SearchByTitleInput = observer(() => {
    const { setFilterTitle, getList, state: { filter: { title }}} = useLinkStore();
    const getListDebounced = useDebounce(() => getList(), 500)

    const handleOnClear = () => {
        setFilterTitle('');
        getList();
    };

    const handleOnChange = (value: string) => {
        setFilterTitle(value);
        getListDebounced();
    };

    return (
        <FloatingInputWrapper>
            <SearchIcon />
            <FloatingInput
                value={title}
                onChange={handleOnChange}
                placeholder="Search by name"
                pl="pl-10" pr="pr-16"/>
            { title.length > 0 && (
                <Button onClick={handleOnClear} className="absolute right-2.5 bottom-2.5 dark:bg-transparent hover:bg-gray-100
                                   px-2 top-3 outline-none">
                    Clear
                </Button>
            )}
        </FloatingInputWrapper>
    );
});

export default SearchByTitleInput;