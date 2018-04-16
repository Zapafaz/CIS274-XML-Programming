(: get only classes on the second floor (room # 200-299 is second floor right?) :)

let $classes := (doc("file:///H:/Projects/School/CIS274-XML-Programming/CIS274_XML_Programming_Project/CIS274_XML_Programming_Project/Resources/XML/SummerBase.xml")/SheetSet/CourseSchedule/Class)
return <SecondFloorClasses>
{
for $x in $classes
where $x/RM >= 200
  and $x/RM <= 299
order by $x/SUBJ
return 
    <Class room="{data($x/RM)}">
        <Title>{data($x/TITLE)}</Title>
        <Course>{data($x/SUBJ)}{data($x/CRSE)}</Course>
        <CRN>{data($x/CRN)}</CRN>
        <Cost>${data($x/COURSE_COST)}.00</Cost>
    </Class>
}
</SecondFloorClasses>
