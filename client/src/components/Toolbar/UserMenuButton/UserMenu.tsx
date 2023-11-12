import useClickOutsideHandler from '../../../hooks/useClickOutsideHandler.ts';
import {observer} from 'mobx-react-lite';
import {useUserStore} from '../../../contexts/AppContext.tsx';

type UserMenuProps = {
    onClose: () => void;
};

const UserMenu = observer(({ onClose }: UserMenuProps) => {
    const { state: { info }, signOut } = useUserStore();
    const refObject = useClickOutsideHandler(() => onClose());
    return (
        <div ref={refObject} className="absolute right-10 z-10 bg-white divide-y divide-gray-100 rounded-lg shadow w-44 dark:bg-gray-700 dark:divide-gray-600">
            <div className="px-4 py-3 text-sm text-gray-900 dark:text-white">
                <div className="font-medium truncate">{info?.nickname}</div>
            </div>
            <ul className="py-2 text-sm text-gray-700 dark:text-gray-200" aria-labelledby="dropdownUserAvatarButton">
                <li>
                    <a href="#" className="block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">Settings</a>
                </li>
            </ul>
            <div className="py-2">
                <button onClick={() => signOut()} role="menuitem" className="flex w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white">Sign out</button>
            </div>
        </div>
    );
});

export default UserMenu;