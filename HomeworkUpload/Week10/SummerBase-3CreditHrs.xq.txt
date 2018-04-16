(: get only 3 credit courses :)

let $classes := (doc("file:///H:/Projects/School/CIS274-XML-Programming/CIS274_XML_Programming_Project/CIS274_XML_Programming_Project/Resources/XML/SummerBase.xml")/SheetSet/CourseSchedule/Class)
return <ThreeCreditClasses>
{
for $x in $classes
where $x/CRDT_HRS = 3
order by $x/SUBJ
return 
    <Class>
        <Title>{data($x/TITLE)}</Title>
        <Course>{data($x/SUBJ)}{data($x/CRSE)}</Course>
        <CRN>{data($x/CRN)}</CRN>
    </Class>
}
</ThreeCreditClasses>
