(: get only Monday classes; not Monday + Wednesday or anything else :)

let $classes := (doc("file:///H:/Projects/School/CIS274-XML-Programming/CIS274_XML_Programming_Project/CIS274_XML_Programming_Project/Resources/XML/SummerBase.xml")/SheetSet/CourseSchedule/Class)
return <MondayClasses>
{
for $x in $classes
where exists($x/MON)
  and not(exists($x/TUE))
  and not(exists($x/WED))
  and not(exists($x/THU))
  and not(exists($x/FRI))
  and not(exists($x/SAT))
order by $x/SUBJ
return 
    <Class>
        <Title>{data($x/TITLE)}</Title>
        <Course>{data($x/SUBJ)}{data($x/CRSE)}</Course>
        <CRN>{data($x/CRN)}</CRN>
        <Cost>${data($x/COURSE_COST)}.00</Cost>
    </Class>
}
</MondayClasses>
