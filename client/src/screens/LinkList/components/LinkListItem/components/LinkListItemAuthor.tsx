import Link from '../../../../../services/LinkService/interfaces/Link.ts';

const LinkListItemAuthor = ({ user, createdAt }: Pick<Link, 'user' | 'createdAt'>) => (
    <p className="secondary-text-color text-sm italic font-light ">
        Added by {user} at { new Date(createdAt).toLocaleDateString("en-US", { year: 'numeric', month: 'long', day: 'numeric' }) }
    </p>
);

export default LinkListItemAuthor;